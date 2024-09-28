using AutoMapper;
using CCG.Application.Contracts.Identity;
using CCG.Application.Exteptions;
using CCG.Domain.Entities.Identity;
using CCG.Shared.Api;
using CCG.Shared.Api.Identity;
using Microsoft.AspNetCore.Identity;

namespace CCG.Application.Services.Identity
{
    public class IdentityService(
        IMapper mapper,
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        IIdentityProviderService identityProviderService) : IIdentityService
    {
        public async Task<UserDataModel> RegisterAsync(string userName, string password)
        {
            var user = new UserEntity
            {
                UserName = userName
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors.Select(x => $"{x.Code} {x.Description}")));

            return await LoginAsync(userName, password);
        }
        
        public async Task<UserDataModel> LoginAsync(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                throw new UnauthorizedException("Invalid credentials.");

            var result = await signInManager.PasswordSignInAsync(user.UserName, password, false, true);
            if (!result.Succeeded)
                throw new UnauthorizedException(result.IsLockedOut
                    ? "User is locked out."
                    : "Invalid credentials.");
            
            await identityProviderService.UpdateTokenAsync(user);
            return mapper.Map<UserDataModel>(user);
        }
    }
}