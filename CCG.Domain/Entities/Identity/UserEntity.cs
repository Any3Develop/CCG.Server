using CCG.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CCG.Domain.Entities.Identity
{
    public class UserEntity : IdentityUser, IEntity<string>
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpireAt { get; set; }
        public DateTime Created { get; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }
}