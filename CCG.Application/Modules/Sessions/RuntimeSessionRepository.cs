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

        public bool TryAdd(ISession runtimeSession)
        {
            return sessions.TryAdd(runtimeSession?.Id, runtimeSession);
        }

        public bool Remove(string id)
        {
            return sessions.TryRemove(id, out _);
        }

        public bool TryRemove(string id, out ISession result)
        {
            return sessions.TryRemove(id, out result);
        }

        public bool Remove(ISession runtimeSession)
        {
            return Remove(runtimeSession?.Id);
        }
    }
}