using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CCG.Application.Contracts.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace CCG.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        public ClaimsPrincipal Claims => httpContextAccessor.HttpContext?.User;
        
        public string UserId => Claims?.FindFirstValue(JwtRegisteredClaimNames.NameId);

        public string UserName => Claims?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string Token => httpContextAccessor.HttpContext?.GetTokenAsync("Bearer", "access_token").GetAwaiter().GetResult();

        public List<string> Roles => Claims?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToList();
    }
}