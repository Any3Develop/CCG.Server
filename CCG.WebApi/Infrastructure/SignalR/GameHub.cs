using CCG.Application.Contracts.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CCG.WebApi.Infrastructure.SignalR
{
    [Authorize]
    public class GameHub(IIdentityProviderService identityProviderService) : Hub
    {
        public async Task<string> GetUserInfo()
        {
            var user = await identityProviderService.ExtractUserFromBearerAsync();
            return $"Authorized user : {user.UserName} with token {user.AccessToken}";
        }
    }
}