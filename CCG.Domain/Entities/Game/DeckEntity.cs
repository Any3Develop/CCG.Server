using System.Text.Json.Serialization;
using CCG.Domain.Entities.Identity;

namespace CCG.Domain.Entities.Game
{
    public class DeckEntity : EntityBase
    {
        public string UserId { get; set; }
        public string HeroId { get; set; }
        public List<string> CardIds { get; set; } = new();

        [JsonIgnore] public virtual UserEntity User { get; set; }
    }
}