using CCG.Shared.Abstractions.Game.Context;

namespace CCG.Application.Modules.Sessions
{
    public class CCGSession(IContext context) : ISession
    {
        public string Id => Context?.RuntimeData?.Id;
        public IContext Context { get; } = context;

        public void Start()
        {
            if (Context.RuntimeData.IsStarted)
                throw new InvalidOperationException($"Can't start session twice : {Context.RuntimeData.Id}");
            
            Context.RuntimeData.StartTime = Context.SharedTime.Current;
            // TODO setup game and wait for an action from players
            // TODO Callbacks
        }

        public void Ready()
        {
            if (Context.RuntimeData.IsReady)
                throw new InvalidOperationException($"Can't ready session again : {Context.RuntimeData.Id}");
            
            Context.RuntimeData.ReadyTime = Context.SharedTime.Current;
            // TODO make an action for start game
            // TODO Callbacks
        }

        public void End()
        {
            if (Context.RuntimeData.IsEnded)
                throw new InvalidOperationException($"Can't end session twice : {Context.RuntimeData.Id}");
            
            Context.RuntimeData.EndTime = Context.SharedTime.Current;
            // TODO make an action for end the game, block all actions
            // TODO Callbacks
        }
    }
}