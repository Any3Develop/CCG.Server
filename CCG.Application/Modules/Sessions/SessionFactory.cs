using AutoMapper;
using CCG.Application.Contracts.Sessions;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Abstractions.Game.Context;
using CCG.Shared.Abstractions.Game.Context.Session;
using CCG.Shared.Abstractions.Game.Factories;
using CCG.Shared.Game.Context.Session;
using CCG.Shared.Game.Runtime.Models;

namespace CCG.Application.Modules.Sessions
{
    public class SessionFactory(
        IMapper mapper,
        ISharedTime sharedTime, 
        ISharedConfig config, 
        IDatabase database,
        IContextFactory contextFactory
        ) : ISessionFactory
    {
        public ISession Create(SessionEntity session)
        {
            var context = CreateContext();
            Init(context, session);
            return new CCGSession(context);
        }

        private CCGContext CreateContext()
        {
            var eventsSource = contextFactory.CreateEventsSource();
            var eventPublisher = contextFactory.CreateEventPublisher(eventsSource);
            var objectsCollection = contextFactory.CreateObjectsCollection(eventPublisher);

            var randomProvider = contextFactory.CreateRuntimeRandomProvider();
            var orderProvider = contextFactory.CreateRuntimeOrderProvider();
            var idProvider = contextFactory.CreateRuntimeIdProvider();
            
            var playersCollection = contextFactory.CreatePlayersCollection();
            var statsFactory = contextFactory.CreateStatFactory(objectsCollection, idProvider);
            var gameQueueCollector = contextFactory.CreateGameQueueCollector(eventsSource, eventPublisher, orderProvider);
            
            var context = new CCGContext
            {
                SharedTime = sharedTime,
                Config = config,
                Database = database,
                ObjectsCollection = objectsCollection,
                PlayersCollection = playersCollection,
                RuntimeRandomProvider = randomProvider,
                RuntimeOrderProvider = orderProvider,
                RuntimeIdProvider = idProvider,
                
                ObjectEventProcessor = contextFactory.CreateObjectEventProcessor(gameQueueCollector),
                ContextEventProcessor = contextFactory.CreateContextEventProcessor(eventsSource, idProvider, orderProvider, randomProvider),
                GameQueueCollector = gameQueueCollector,
                EventPublisher = eventPublisher,
                EventSource = eventsSource,

                ObjectFactory = contextFactory.CreateObjectFactory(objectsCollection, idProvider, statsFactory),
                PlayerFactory = contextFactory.CreatePlayerFactory(playersCollection, idProvider, statsFactory),
                EffectFactory = contextFactory.CreateEffectFactory(objectsCollection, idProvider),
                StatFactory = statsFactory,
                ContextFactory = contextFactory,
            };
            
            context.GameEventProcessor = contextFactory.CreateGameEventProcessor(context);
            context.CommandProcessor = contextFactory.CreateCommandProcessor(context);
            
            return context;
        }

        private void Init(CCGContext context, SessionEntity sessionEntity)
        {
            var contextModel = new RuntimeContextModel
            {
                Id = sessionEntity.Id,
                Players = mapper.Map<List<SessionPlayer>>(sessionEntity.Players)
            };

            context.Sync(contextModel);
            context.RuntimeIdProvider.Sync(new RuntimeIdModel());
            context.RuntimeOrderProvider.Sync(new RuntimeOrderModel());
            context.RuntimeRandomProvider.Sync(new RuntimeRandomModel());
        }
    }
}