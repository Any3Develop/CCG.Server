using CCG.Domain.Entities.Lobby;
using CCG.Shared.Abstractions.Game.Context.Session;
using CCG.Shared.Abstractions.Game.Runtime.Models;

namespace CCG.Application.Contracts.Sessions
{
    public interface ISessionFactory
    {
        ISession Create(SessionEntity session);
        ISession Create(IContextModel[] models);
    }
}