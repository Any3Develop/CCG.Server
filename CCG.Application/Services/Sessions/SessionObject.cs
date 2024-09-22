using CCG.Shared.Abstractions.Game.Context.Session;
using CCG.Shared.Game.Context.Session;

namespace CCG.Application.Services.Sessions
{
    public class SessionObject
    {
        public ISession Session { get; }
        public string Id => Session?.Id;
        public SessionPlayerModel[] Players => Session?.Players;
        public DateTime? StartTime { get; private set; }

        public SessionObject(IEnumerable<SessionPlayerModel> players, string sessionId) {}

        public void Start()
        {
            if (StartTime.HasValue)
                throw new Exception($"Can't start session twice : {Id}");
            StartTime = DateTime.UtcNow;
            Session.Build(); // TODO
        }
    }
}