namespace PaleLuna.Architecture
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

