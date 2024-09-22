using System.Text.Json.Serialization;
using CCG.Domain.Entities.Game;
using CCG.Domain.Entities.Identity;

namespace CCG.Domain.Entities.Lobby
{
    public class LobbyPlayerEntity : EntityBase
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DeckId { get; set; }
        public string DuelId { get; set; }
        public string SessionId { get; set; }

        [JsonIgnore] public virtual UserEntity User { get; set; }
        [JsonIgnore] public virtual DeckEntity Deck { get; set; }
        [JsonIgnore] public virtual DuelEntity Duel { get; set; }
        [JsonIgnore] public virtual SessionEntity Session { get; set; }
    }
}