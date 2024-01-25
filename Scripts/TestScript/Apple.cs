using PaleLuna.Architecture.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item, IService
{
    public Apple()
    {
        this.name = "apple";
    }

    public string EatApple()
    {
        return "Eat it!";
    }
}
