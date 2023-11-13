public abstract class GameStateBase : State
{
    protected GameController _context;

    public GameStateBase(GameController context) =>
        _context = context;
}