using CCG.Shared.Abstractions.Game.Context.Session;

namespace CCG.Application.Contracts.Sessions
{
    public interface IRuntimeSessionRepository
    {
        ISession Get(string id);
        void Add(ISession runtimeSession);
        bool Remove(string id);
        bool Remove(ISession runtimeSession);
    }
}