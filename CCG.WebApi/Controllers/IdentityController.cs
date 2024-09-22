using AutoMapper;
using CCG.Application.Contracts.Identity;
using CCG.Domain.Entities.Identity;
using CCG.Shared.Api;
using CCG.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CCG.WebApi.Controllers
{
    [Route("api/" + VersionInfo.ApiVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController(
        IMapper mapper,
        IIdentityProviderService identityProviderService,
        SignInManager<UserEntity> signInManager,
        UserManager<UserEntity> userManager) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(string userName, string password)
        {
            var user = new UserEntity
            {
                UserName = userName
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return await Login(userName, password);
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var result = await signInManager.PasswordSignInAsync(user.UserName, password, false, true);
            if (!result.Succeeded)
                return Unauthorized(result.IsLockedOut
                    ? "User is locked out."
                    : "Invalid credentials.");
            
            await identityProviderService.UpdateTokenAsync(user);
            return Ok(mapper.Map<UserData>(user));
        }
    }
}