using NUnit.Framework;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Dictinory;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IStartable
{
    [SerializeField] private string _name;

    [SerializeField] private Test other;


    private bool _isStartable = false;

    public bool IsStarted => _isStartable;

    public void OnStart()
    {
        if (_isStartable) return;

        RunTests();

        _isStartable = true;
    }

    private void RunTests()
    {
        List<Item> items = new() { new Apple("Green"), new Apple("Yellow"), new Rock() };
        DataHolder<Item> itemsNew = new(items);

        items.ForEach(item => print(item.GetName()));
    }
}