using System.Collections.Concurrent;
using CCG.Application.Contracts.Services.Sessions;

namespace CCG.Application.Services.Sessions
{
    public class RuntimeSessionRepository : IRuntimeSessionRepository
    {
        private readonly ConcurrentDictionary<string, SessionObject> sessions = new();

        public SessionObject Get(string id)
        {
            return sessions[id];
        }

        public void Add(SessionObject sessionObject)
        {
            sessions[sessionObject.Id] = sessionObject;
        }

        public bool Remove(string id)
        {
            return sessions.TryRemove(id, out _);
        }

        public bool Remove(SessionObject sessionObject)
        {
            return Remove(sessionObject?.Id);
        }
    }
}