using CCG.Shared.Abstractions.Game.Context;
using CCG.Shared.Abstractions.Game.Factories;
using CCG.Shared.Abstractions.Game.Runtime.Models;
using CCG.Shared.Game.Runtime.Models;

namespace CCG.Application.Modules.Sessions
{
    public class SessionFactory(
        IContextFactory contextFactory,
        IContextInitializer contextInitializer
        ) : ISessionFactory
    {
        public ISession Create(string id, List<SessionPlayer> players)
        {
            var context = contextFactory.CreateContext();
            contextInitializer.Init(context, id, players);
            return CreateInternal(context);
        }

        public ISession Create(IContextModel[] models)
        {
            var context = contextFactory.CreateContext();
            contextInitializer.Init(context, models);
            return CreateInternal(context);
        }

        protected virtual ISession CreateInternal(IContext context)
        {
            return new CCGSession(context);
        }
    }
}