using System.Text.Json.Serialization;
using CCG.Domain.Contracts;
using CCG.Domain.Entities.Game;
using CCG.Domain.Entities.Lobby;
using Microsoft.AspNetCore.Identity;

namespace CCG.Domain.Entities.Identity
{
    public class UserEntity : IdentityUser, IEntity<string>
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpireAt { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;

        [JsonIgnore] public virtual List<DeckEntity> Decks { get; set; } = new();
        [JsonIgnore] public virtual List<LobbyPlayerEntity> Players { get; set; }
    }
}