using CCG.Shared.Abstractions.Game.Context.Session;
using CCG.Shared.Api.Game;
using CCG.Shared.Game.Context.Session;

namespace CCG.Application.Modules.Sessions
{
    public class SessionObject(ISession session)
    {
        public ISession Session { get; } = session;
        public string Id => session?.Id;
        public SessionPlayer[] Players => session?.Players;
        public DateTime? StartTime { get; private set; }

        public void Start()
        {
            if (StartTime.HasValue)
                throw new Exception($"Can't start session twice : {Id}");
            
            StartTime = DateTime.UtcNow;
            Session.Build(); // TODO
        }
    }
}