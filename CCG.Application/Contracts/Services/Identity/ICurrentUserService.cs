using System.Security.Claims;

namespace CCG.Application.Contracts.Services.Identity
{
    public interface ICurrentUserService
    {
        public ClaimsPrincipal Claims { get; }
        public string UserId { get; }
        public string UserName { get; }
        public string Token { get; }
        public List<string> Roles { get; }
    }
}