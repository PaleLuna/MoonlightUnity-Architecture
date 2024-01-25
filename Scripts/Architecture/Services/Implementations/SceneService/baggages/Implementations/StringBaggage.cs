namespace PaleLuna.Architecture.Services
{
    public class StringBaggage : BaggageBase<string>
    {
        public StringBaggage(string value) => this.SetBaggage(value);
    }
}