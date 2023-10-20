public class StartState : GameStateBase
{
    public StartState(GameController context) : base(context) { }
    public override void StateStart() =>
        _context.startableHolder
            .ForEach(startable => startable.OnStart());
}