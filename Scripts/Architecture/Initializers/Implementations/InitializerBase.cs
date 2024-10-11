namespace PaleLuna.Architecture.Initializer
{
    public abstract class InitializerBase : IInitializer
    {
        protected InitStatus _status = InitStatus.Shutdown;
        public InitStatus status => _status;

        public abstract void StartInit();
    }
}

