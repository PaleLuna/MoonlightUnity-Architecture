using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item
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
