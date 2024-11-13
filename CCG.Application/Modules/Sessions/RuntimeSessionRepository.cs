using System.Collections.Concurrent;
using CCG.Application.Contracts.Sessions;
using CCG.Shared.Abstractions.Game.Context;

namespace CCG.Application.Modules.Sessions
{
    public class RuntimeSessionRepository : IRuntimeSessionRepository
    {
        private readonly ConcurrentDictionary<string, ISession> sessions = new();

        public ISession Get(string id)
        {
            return sessions[id];
        }

        public void Add(ISession runtimeSession)
        {
            sessions[runtimeSession.Id] = runtimeSession;
        }

        public bool Remove(string id)
        {
            return !string.IsNullOrEmpty(id) && sessions.TryRemove(id, out _);
        }

        public bool Remove(ISession runtimeSession)
        {
            return Remove(runtimeSession?.Id);
        }
    }
}