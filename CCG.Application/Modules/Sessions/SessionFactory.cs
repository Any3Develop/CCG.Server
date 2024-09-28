using AutoMapper;
using CCG.Application.Contracts.Sessions;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Api.Game;
using CCG.Shared.Game.Context.Session;

namespace CCG.Application.Modules.Sessions
{
    public class SessionFactory(IMapper mapper) : ISessionFactory
    {
        public SessionObject Create(SessionEntity session)
        {
            var sessionPlayers = mapper.Map<List<SessionPlayer>>(session.Players);
            return new SessionObject(null); // TODO create session
        }
    }
}