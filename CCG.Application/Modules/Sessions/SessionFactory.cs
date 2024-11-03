using AutoMapper;
using CCG.Application.Contracts.Sessions;
using CCG.Domain.Entities.Lobby;
using CCG.Shared.Abstractions.Game.Context;
using CCG.Shared.Abstractions.Game.Context.Session;
using CCG.Shared.Abstractions.Game.Factories;
using CCG.Shared.Abstractions.Game.Runtime.Models;
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
            InitContext(context, session.Id, mapper.Map<List<SessionPlayer>>(session.Players));
            return new CCGSession(context);
        }
        
        public ISession Create(IContextModel[] models)
        {
            var context = CreateContext();
            InitContext(context, models);
            return new CCGSession(context);
        }

        private CCGContext CreateContext()
        {
            var eventsSource = contextFactory.CreateEventsSource();
            var eventPublisher = contextFactory.CreateEventPublisher(eventsSource);
            var objectsCollection = contextFactory.CreateObjectsCollection(eventPublisher);
            var playersCollection = contextFactory.CreatePlayersCollection();

            var randomProvider = contextFactory.CreateRuntimeRandomProvider();
            var orderProvider = contextFactory.CreateRuntimeOrderProvider();
            var idProvider = contextFactory.CreateRuntimeIdProvider();

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
            context.TimerFactory = contextFactory.CreateTimerFactory(context);
            return context;
        }

        private void InitContext(CCGContext context, string id, List<SessionPlayer> players)
        {
            var models = new List<IContextModel>
            {
                new RuntimeContextModel {Id = id, Players = players},
                new RuntimeIdModel(),
                new RuntimeOrderModel(),
                new RuntimeRandomModel(),
                context.TimerFactory.CreateModel(),
            };
            models.AddRange(players.Select((p, i) => context.PlayerFactory.CreateModel(p.Id, i)));

            InitContext(context, models);
            context.RuntimeRandomProvider.Next();
        }
        
        private void InitContext(CCGContext context, IReadOnlyCollection<IContextModel> models)
        {
            context.Sync(GetModel<IRuntimeContextModel>(models));
            context.RuntimeIdProvider.Sync(GetModel<IRuntimeIdModel>(models));
            context.RuntimeOrderProvider.Sync(GetModel<IRuntimeOrderModel>(models));
            context.RuntimeRandomProvider.Sync(GetModel<IRuntimeRandomModel>(models));
            context.RuntimeTimer = context.TimerFactory.Create(GetModel<IRuntimeTimerModel>(models), false);
            
            var players = models.OfType<IRuntimePlayerModel>().Select(model => context.PlayerFactory.Create(model, false));
            context.PlayersCollection.AddRange(players, false);
        }

        private static TModel GetModel<TModel>(IReadOnlyCollection<IContextModel> models, int? index = null)
        {
            if (!index.HasValue)
                return (TModel) models.FirstOrDefault(x => x is TModel);

            return models.OfType<TModel>().ElementAt(Math.Clamp(index.Value, 0, models.Count));
        }
    }
}