using CCG.Application.Modules.Sessions;
using CCG.Domain.Entities.Lobby;

namespace CCG.Application.Contracts.Sessions
{
    public interface ISessionFactory
    {
        SessionObject Create(SessionEntity session);
    }
}