using AutoMapper;
using CCG.Application.Contracts.Persistence;
using CCG.Application.Contracts.Services.Sessions;
using CCG.Application.Exteptions;
using CCG.Domain.Entities.Identity;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Api.Lobby;
using Microsoft.EntityFrameworkCore;

namespace CCG.Application.Services.Lobby
{
    public class LobbyService(
        IMapper mapper,
        IAppDbContext dbContext,
        ISessionFactory sessionFactory,
        IRuntimeSessionRepository runtimeSessionRepository)
    {
        public async Task<DuelModel> DuelCreateAsync(UserEntity userHost, string name)
        {
            await DuelCloseAsync(userHost);

            var hostPlayer = CreatePlayer(userHost);
            var duelEntity = new DuelEntity
            {
                HostId = userHost.Id,
                Name = name
            };

            duelEntity.Players.Add(hostPlayer);
            await dbContext.Players.AddAsync(hostPlayer);
            await dbContext.Duels.AddAsync(duelEntity);
            await dbContext.SaveChangesAsync();

            return mapper.Map<DuelModel>(duelEntity);
        }

        public async Task<DuelModel> DuelJoinAsync(UserEntity userJoin, string duelId)
        {
            await DuelCloseAsync(userJoin);
            
            var duelEntity = await dbContext.Duels
                .Where(x => x.Id == duelId && x.Players.Count < 2 && !x.Closed.HasValue)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();

            if (duelEntity == null)
                throw new NotFoundException($"Can't joint to the duel, duel is not available.");

            if (duelEntity.Players.Any(x => x.UserId == userJoin.Id))
                throw new ValidationException("You has already joined int this duel.");
            
            var joinPlayer = CreatePlayer(userJoin);
            duelEntity.Players.Add(joinPlayer);
            await dbContext.Players.AddAsync(joinPlayer);

            return mapper.Map<DuelModel>(duelEntity);
        }

        public async Task<SessionModel> DuelSessionStartAsync(UserEntity userHost)
        {
            var duelEntity = await dbContext.Duels
                .Where(x => x.HostId == userHost.Id && x.Players.Count >= 2 && !x.Closed.HasValue)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();
            
            if (duelEntity == null)
                throw new NotFoundException($"Can't start the duel, duel is not available.");
            
            if (duelEntity.Players.Count < 2)
                throw new ValidationException($"Can't start the duel, not enough players.");
            
            duelEntity.Closed = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            
            var sessionEntity = await CreateSessionAsync(duelEntity.Players.ToList());
            
            duelEntity.Players.Clear();
            dbContext.Duels.Remove(duelEntity);
            await dbContext.SaveChangesAsync();
            
            return await StartSessionAsync(sessionEntity);
        }

        public async Task<SessionEntity> CreateSessionAsync(List<LobbyPlayerEntity> players)
        {
            var sessionEntity = new SessionEntity
            {
                Players = players
            };

           await dbContext.Sessions.AddAsync(sessionEntity);
           await dbContext.SaveChangesAsync();
           
           runtimeSessionRepository.Add(sessionFactory.Create(sessionEntity));
           return sessionEntity;
        }

        public async Task<SessionModel> StartSessionAsync(SessionEntity session)
        {
            if (session.StartTime.HasValue)
                throw new ValidationException($"Can't start session twice : {session.Id}");
            
            var sessionObject = runtimeSessionRepository.Get(session.Id);
            sessionObject.Start();

            session.StartTime = sessionObject.StartTime;
            await dbContext.SaveChangesAsync();
            
            return mapper.Map<SessionModel>(session);
        }

        public async Task DuelCloseAsync(UserEntity userHost)
        {
            var duelsToClose = await dbContext.Duels
                .Where(x => x.HostId == userHost.Id)
                .ToListAsync();

            if (duelsToClose.Count == 0)
                return;
            
            foreach (var duelEntity in duelsToClose)
                duelEntity.Players.Clear();
            
            dbContext.Duels.RemoveRange(duelsToClose);
            await dbContext.SaveChangesAsync();
        }

        public LobbyPlayerEntity CreatePlayer(UserEntity user)
        {
            var deckEntity = user.Decks.First();
            var playerEntity = mapper.Map<LobbyPlayerEntity>(user);
            playerEntity.DeckId = deckEntity.Id;
            playerEntity.Deck = deckEntity;

            return playerEntity;
        }
    }
}