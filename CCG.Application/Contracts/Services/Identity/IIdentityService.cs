using CCG.Shared.Api;

namespace CCG.Application.Contracts.Services.Identity
{
    public interface IIdentityService
    {
        Task<UserData> RegisterAsync(string userName, string password);
        Task<UserData> LoginAsync(string userName, string password);
    }
}