using CCG.Domain.Entities.Identity;

namespace CCG.Application.Contracts.Identity
{
    public interface IIdentityProviderService
    {
        Task<UserEntity> UpdateToken(UserEntity user);

        Task<UserEntity> ExtractUserFromBearerAsync();
    }
}