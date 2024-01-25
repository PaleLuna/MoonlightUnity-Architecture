namespace PaleLuna.Architecture.Services
{
    public class BoolBaggage : BaggageBase<bool>
    {
        public BoolBaggage(bool value) => SetBaggage(value);
    }
}