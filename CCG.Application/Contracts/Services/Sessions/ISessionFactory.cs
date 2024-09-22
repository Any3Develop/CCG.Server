using CCG.Application.Services.Sessions;
using CCG.Domain.Entities.Lobby;

namespace CCG.Application.Contracts.Services.Sessions
{
    public interface ISessionFactory
    {
        SessionObject Create(SessionEntity session);
    }
}