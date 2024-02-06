using PaleLuna.Architecture.Services;

public class Apple : Item, IService
{
    private string _color;

    public string color => _color;

    public Apple(string color)
    {
        this.name = "apple";
        this._color = color;
    }

    public string EatApple()
    {
        return "Eat it!";
    }
}
