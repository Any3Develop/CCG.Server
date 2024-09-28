using CCG.Shared.Api;
using CCG.Shared.Api.Identity;

namespace CCG.Application.Contracts.Identity
{
    public interface IIdentityService
    {
        Task<UserDataModel> RegisterAsync(string userName, string password);
        Task<UserDataModel> LoginAsync(string userName, string password);
    }
}