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
            var sessionPlayers = mapper.Map<List<SessionPlayer>>(session.Players);
            var contextModel = new RuntimeContextModel
            {
                Id = session.Id,
                Players = sessionPlayers
            };
            
            return new CCGSession(CreateContext().Sync(contextModel));
        }

        private CCGContext CreateContext()
        {
            var contextEventSource = contextFactory.CreateEventsSource();
            var contextEventPublisher = contextFactory.CreateEventPublisher(contextEventSource);
            var objectsCollection = contextFactory.CreateObjectsCollection(contextEventPublisher);
            var runtimeIdProvider = contextFactory.CreateRuntimeIdProvider();
            
            var context = new CCGContext
            {
                SharedTime = sharedTime,
                Config = config,
                Database = database,
                ObjectsCollection = objectsCollection,
                PlayersCollection = contextFactory.CreatePlayersCollection(),
                RuntimeOrderProvider = contextFactory.CreateRuntimeOrderProvider(),
                RuntimeRandomProvider = contextFactory.CreateRuntimeRandomProvider(),
                RuntimeIdProvider = runtimeIdProvider,
                EventPublisher = contextEventPublisher,
                EventSource = contextEventSource,
                ContextFactory = contextFactory,
            };

            var commandFactory = contextFactory.CreateCommandFactory(context);
            var gameQueueCollector = contextFactory.CreateGameQueueCollector(context, contextEventSource);
            var statsFactory = contextFactory.CreateStatFactory(database, objectsCollection, runtimeIdProvider);
            
            context.GameQueueCollector = gameQueueCollector;
            context.CommandProcessor = contextFactory.CreateCommandProcessor(context, commandFactory);
            context.StatFactory = contextFactory.CreateStatFactory(database, objectsCollection, runtimeIdProvider);
            context.ObjectFactory = contextFactory.CreateObjectFactory(database, objectsCollection, runtimeIdProvider, statsFactory);
            context.EffectFactory = contextFactory.CreateEffectFactory(database, objectsCollection, runtimeIdProvider);
            context.ObjectEventProcessor = contextFactory.CreateObjectEventProcessor(context, gameQueueCollector);
            context.ContextEventProcessor = contextFactory.CreateContextEventProcessor(context);
            context.GameEventProcessor = contextFactory.CreateGameEventProcessor(context);
            
            return context;
        }
    }
}