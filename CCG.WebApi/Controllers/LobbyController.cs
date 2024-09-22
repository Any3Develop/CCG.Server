using CCG.Application.Contracts.Services.Identity;
using CCG.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCG.WebApi.Controllers
{
    [Route("api/" + VersionInfo.ApiVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController(IIdentityProviderService identityProviderService) : ControllerBase
    {

    }
}