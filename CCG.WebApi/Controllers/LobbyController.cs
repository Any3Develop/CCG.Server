using CCG.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCG.WebApi.Controllers
{
    [Route("api/" + VersionInfo.ApiVersion + "/[controller]")]
    [ApiController]
    [Authorize]
    public class LobbyController : ControllerBase
    {
        [HttpGet(nameof(GetLobbyInfi))]
        public IActionResult GetLobbyInfi()
        {
            return Ok("It works.");
        }
    }
}