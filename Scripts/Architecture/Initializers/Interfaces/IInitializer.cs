namespace PaleLuna.Architecture.Initializer
{
    public interface IInitializer
    {
        public InitStatus status { get; }
        public void StartInit();
    }

    public enum InitStatus
    {
        Shutdown,
        Initialization,
        Done
    }
}

