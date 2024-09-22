using System.Text.Json.Serialization;

namespace CCG.Domain.Entities.Lobby
{
    public class DuelEntity : EntityBase
    {
        public string Name { get; set; }
        public string HostId { get; set; }
        public DateTime? Closed { get; set; }

        [JsonIgnore] public virtual List<LobbyPlayerEntity> Players { get; set; } = new();
    }
}