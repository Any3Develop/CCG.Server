using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CCG.Application.Contracts.Identity;
using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CCG.Infrastructure.Services
{
    public class IdentityProviderService(
		    IAppDbContext dbContext,
		    JwtTokenConfig jwtTokenConfig,
		    UserManager<UserEntity> userManager,
		    ICurrentUserService currentUserService) 
	    : IIdentityProviderService
	{
		public async Task<UserEntity> UpdateToken(UserEntity user)
		{
			user.AccessTokenExpireAt = DateTime.UtcNow.AddYears(1);
			user.AccessToken = await GenerateJwtTokenAsync(user);
			await dbContext.SaveChangesAsync();
			return user;
		}

		public async Task<UserEntity> ExtractUserFromBearerAsync()
        {
	        if (string.IsNullOrWhiteSpace(currentUserService.Token)) 
		        throw new Exception("Application token is empty");
	        
	        if (IsAccessTokenValid(currentUserService.Token)) 
		        throw new Exception("Access token has expired.");

	        var user = await userManager.GetUserAsync(currentUserService.Claims);

            if (user is null)
	            throw new Exception("User not found by application token data");
            
            return user;
        }
		
		private bool IsAccessTokenValid(string token)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			var tokenExpiresDate = jwtTokenHandler.ReadToken(token).ValidTo;

			return tokenExpiresDate <= DateTime.UtcNow;
		}
		
		private async Task<string> GenerateJwtTokenAsync(UserEntity user)
		{
			var claims = new List<Claim> {
				new(ClaimTypes.NameIdentifier, user.UserName),
				new(JwtRegisteredClaimNames.NameId, user.Id),
				new(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new(JwtRegisteredClaimNames.Sub, user.UserName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await userManager.GetRolesAsync(user);
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var jwtToken = new JwtSecurityToken(
				jwtTokenConfig.Issuer,
				jwtTokenConfig.Issuer,
				claims,
				expires: user.AccessTokenExpireAt,
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
					SecurityAlgorithms.HmacSha256Signature));

			return new JwtSecurityTokenHandler().WriteToken(jwtToken);
		}
	}
}