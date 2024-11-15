using CCG.Shared.Abstractions.Game.Context;

namespace CCG.Application.Contracts.Sessions
{
    public interface IRuntimeSessionRepository
    {
        ISession Get(string id);
        void Add(ISession runtimeSession);
        bool TryAdd(ISession runtimeSession);
        bool Remove(string id);
        bool TryRemove(string id, out ISession result);
        bool Remove(ISession runtimeSession);
    }
}