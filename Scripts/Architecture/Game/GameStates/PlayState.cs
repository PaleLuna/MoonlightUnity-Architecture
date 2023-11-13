
public class PlayState : GameStateBase
{
    public PlayState(GameController context) : base(context) { }

    public override void StateStart() =>
        _context.pausablesHolder
            .ForEach(pausable => pausable.OnResume());
}