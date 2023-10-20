public class PauseState : GameStateBase
{
    public PauseState(GameController context) : base(context) { }

    public override void StateStart() =>
        _context.pausablesHolder
            .ForEach(pausable => pausable.OnPause());
}
