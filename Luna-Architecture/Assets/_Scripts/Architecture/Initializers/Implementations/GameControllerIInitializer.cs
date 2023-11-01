namespace PaleLuna.Architecture
{
    public class GameControllerIInitializer : IInitializer
    {
        private InitStatus _status = InitStatus.Shutdown;
        
        public InitStatus status { get; }
        
        public void Init()
        {
            
        }
    }
}