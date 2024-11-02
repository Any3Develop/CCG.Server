using CCG.Domain.Entities.Lobby;
using CCG.Shared.Abstractions.Game.Context.Session;

namespace CCG.Application.Contracts.Sessions
{
    public interface ISessionFactory
    {
        ISession Create(SessionEntity session);
    }
}