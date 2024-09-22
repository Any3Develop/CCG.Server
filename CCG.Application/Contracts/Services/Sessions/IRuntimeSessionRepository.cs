using CCG.Application.Services.Sessions;

namespace CCG.Application.Contracts.Services.Sessions
{
    public interface IRuntimeSessionRepository
    {
        SessionObject Get(string id);
        void Add(SessionObject sessionObject);
        bool Remove(string id);
        bool Remove(SessionObject sessionObject);
    }
}