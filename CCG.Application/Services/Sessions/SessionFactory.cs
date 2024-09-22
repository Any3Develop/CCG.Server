using AutoMapper;
using CCG.Application.Contracts.Services.Sessions;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Game.Context.Session;

namespace CCG.Application.Services.Sessions
{
    public class SessionFactory(IMapper mapper) : ISessionFactory
    {
        public SessionObject Create(SessionEntity session)
        {
            var sessionPlayers = mapper.Map<List<SessionPlayerModel>>(session.Players);
            return new SessionObject(sessionPlayers, session.Id);
        }
    }
}