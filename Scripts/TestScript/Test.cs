using PaleLuna.Architecture.GameComponent;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Counter;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour, IStartable
{
    [SerializeField] private string _name;

    [SerializeField] private Test other;

    private ObjectCounter<Item> _itemCounter;
    private DataHolder<Test> dataHolder = new();


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
        Rock rock = new Rock();

        dataHolder.Registration(new List<Test>()
        {
            other
        });

        print(dataHolder.At(0));
        print(dataHolder);
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        dataHolder.Clear();

        print(dataHolder);
    }
}