using CCG.Application.Contracts.Services.Identity;
using CCG.Application.Exteptions;
using CCG.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCG.WebApi.Controllers
{
    [Route("api/" + VersionInfo.ApiVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController(IIdentityService identityService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(string userName, string password)
        {
            try
            {
                var result = await identityService.RegisterAsync(userName, password);
                return Ok(result);
            }
            catch (UnauthorizedException e)
            {
                return Unauthorized(e);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(string userName, string password)
        {
            try
            {
                var result = await identityService.LoginAsync(userName, password);
                return Ok(result);
            }
            catch (UnauthorizedException e)
            {
                return Unauthorized(e);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}