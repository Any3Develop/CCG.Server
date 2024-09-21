using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CCG.Application.Contracts.Identity;
using CCG.Application.Contracts.Persistence;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CCG.WebApi.Infrastructure.Services
{
    public class IdentityProviderService(
		    IAppDbContext dbContext,
		    JwtTokenConfig jwtTokenConfig,
		    UserManager<UserEntity> userManager,
		    ICurrentUserService currentUserService) 
	    : IIdentityProviderService
	{
		public async Task<UserEntity> UpdateTokenAsync(UserEntity user)
		{
			user.AccessTokenExpireAt = DateTime.UtcNow.AddYears(1);
			user.AccessToken = await GenerateJwtTokenAsync(user);
			await dbContext.SaveChangesAsync();
			await userManager.UpdateAsync(user);
			return user;
		}

		public async Task<UserEntity> ExtractUserFromBearerAsync()
        {
            return await userManager.GetUserAsync(currentUserService.Claims);
        }
		
		private async Task<string> GenerateJwtTokenAsync(UserEntity user)
		{
			var claims = new List<Claim> {
				new(ClaimTypes.NameIdentifier, user.Id),
				new(JwtRegisteredClaimNames.NameId, user.Id),
				new(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new(JwtRegisteredClaimNames.Sub, user.UserName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await userManager.GetRolesAsync(user);
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var jwtToken = new JwtSecurityToken(
				jwtTokenConfig.Issuer,
				jwtTokenConfig.Audience,
				claims,
				expires: user.AccessTokenExpireAt,
				signingCredentials: new SigningCredentials(
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret)),
					SecurityAlgorithms.HmacSha256Signature));

			return new JwtSecurityTokenHandler().WriteToken(jwtToken);
		}
	}
}