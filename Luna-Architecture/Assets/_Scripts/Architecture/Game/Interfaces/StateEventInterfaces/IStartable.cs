public interface IStartable : IGameComponent
{
    public bool IsStarted { get; }
    void OnStart();
}