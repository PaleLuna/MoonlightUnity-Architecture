using UnityEngine;

public abstract class Item
{
    protected string name;

    public virtual string GetName()
    {
        return name;
    }
}
