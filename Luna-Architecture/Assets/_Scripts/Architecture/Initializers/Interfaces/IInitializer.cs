namespace PaleLuna.Architecture
{
    public interface IInitializer
    {
        public InitStatus status { get; }

        public void Init();
    }
    
    public enum InitStatus
    {
        Shutdown,
        Initialization,
        Done
    }
}

