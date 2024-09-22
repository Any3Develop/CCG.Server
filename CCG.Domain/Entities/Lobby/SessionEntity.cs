using System.Text.Json.Serialization;

namespace CCG.Domain.Entities.Lobby
{
    public class SessionEntity : EntityBase
    {
        public DateTime? StartTime { get; set; }
        
        [JsonIgnore] public virtual List<LobbyPlayerEntity> Players { get; set; } = new();
    }
}