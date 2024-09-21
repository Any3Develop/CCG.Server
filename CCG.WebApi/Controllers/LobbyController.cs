using CCG.Application;
using CCG.Application.Contracts.Identity;
using CCG.Application.Utilities;
using CCG.Domain.Entities.Identity;
using CCG.Infrastructure.Persistence;
using CCG.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CCG.WebApi.Controllers
{
    [Route("api/" + VersionInfo.ApiVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController(IIdentityProviderService identityProviderService, UserManager<UserEntity> userManager) : ControllerBase
    {
        [HttpGet(nameof(AuthAdmin))]
        [AllowAnonymous]
        public async Task<IActionResult> AuthAdmin()
        {
            var user = await userManager.FindByEmailAsync(Constants.AdminEmail);
            if (user == null)
                return Unauthorized($"User [{Constants.AdminEmail}] not found.");

            await identityProviderService.UpdateTokenAsync(user);
            return Ok(user);
        }

        [HttpGet(nameof(TestAdminToken))]
        public async Task<IActionResult> TestAdminToken()
        {
            var user = await identityProviderService.ExtractUserFromBearerAsync();
            return Ok(user);
        }
    }
}