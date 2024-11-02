using CCG.Shared.Abstractions.Game.Collections;
using CCG.Shared.Abstractions.Game.Context;
using CCG.Shared.Abstractions.Game.Context.EventProcessors;
using CCG.Shared.Abstractions.Game.Context.EventSource;
using CCG.Shared.Abstractions.Game.Context.Providers;
using CCG.Shared.Abstractions.Game.Factories;
using CCG.Shared.Abstractions.Game.Runtime.Models;

namespace CCG.Application.Modules.Sessions
{
    public class CCGContext : IContext
    {
        #region Static Context
        public ISharedTime SharedTime { get; set; }
        public ISharedConfig Config { get; set; }
        public IDatabase Database { get; set; }

        #endregion

        #region Runtime Context
        public IRuntimeContextModel RuntimeData { get; private set; }
        public IObjectsCollection ObjectsCollection { get; set; }
        public IPlayersCollection PlayersCollection { get; set; }
        public IRuntimeRandomProvider RuntimeRandomProvider { get; set; }
        public IRuntimeOrderProvider RuntimeOrderProvider { get; set; }
        public IRuntimeIdProvider RuntimeIdProvider { get; set; }
        #endregion

        #region Logic Context
        public IObjectEventProcessor ObjectEventProcessor { get; set; }
        public IContextEventProcessor ContextEventProcessor { get; set; }
        public IGameEventProcessor GameEventProcessor { get; set; }
        public ICommandProcessor CommandProcessor { get; set; }
        public IGameQueueCollector GameQueueCollector { get; set; }
        public IEventPublisher EventPublisher { get; set; }
        public IEventsSource EventSource { get; set; }
        public IRuntimeObjectFactory ObjectFactory { get; set; }
        public IRuntimePlayerFactory PlayerFactory { get; set; }
        public IRuntimeEffectFactory EffectFactory { get; set; }
        public IRuntimeStatFactory StatFactory { get; set; }
        public IContextFactory ContextFactory { get; set; }
        #endregion

        public IContext Sync(IRuntimeContextModel value, bool notify = false)
        {
            RuntimeData = value;
            if (notify)
                EventPublisher.Publish(null); // TODO notify context changed
            
            return this;
        }
    }
}