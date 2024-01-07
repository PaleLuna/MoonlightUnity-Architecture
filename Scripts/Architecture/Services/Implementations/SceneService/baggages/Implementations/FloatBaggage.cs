namespace PaleLuna.Architecture.Services
{
    public class FloatBaggage: BaggageBase<float>
    {
        public FloatBaggage(float value) => this.SetBaggage(value);
    }
}