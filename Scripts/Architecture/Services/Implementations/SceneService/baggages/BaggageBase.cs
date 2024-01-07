namespace PaleLuna.Architecture.Services
{
    public abstract class BaggageBase<T> : IBaggage
    {
        private T _baggage;
        
        public T GetBaggage()
        {
            return _baggage;
        }

        public void SetBaggage(T baggage)
        {
            this._baggage = baggage;
        }
    }
}