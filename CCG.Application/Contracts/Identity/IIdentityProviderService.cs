using CCG.Domain.Entities.Identity;

namespace CCG.Application.Contracts.Identity
{
    public interface IIdentityProviderService
    {
        Task<UserEntity> UpdateTokenAsync(UserEntity user);

        Task<UserEntity> ExtractUserFromBearerAsync();
    }
}