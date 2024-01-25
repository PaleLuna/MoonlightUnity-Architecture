namespace PaleLuna.Architecture.Services
{
    public class IntBaggage : BaggageBase<int>
    {
        public IntBaggage(int value) => this.SetBaggage(value);
    }
}