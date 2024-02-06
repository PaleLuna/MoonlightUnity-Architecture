using PaleLuna.Architecture.GameComponent;
using PaleLuna.DataHolder.Dictinory;
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
        DictinoryHolder<string, Item> dictinory = new();

        dictinory
            .Registration("Apple1", new Apple("red"))
            .Registration("Rock1", new Rock())
            .Registration("Rock2", new Rock());

        print(dictinory.Pop<Apple>("Rock1").color);

        dictinory.ForEach(item => print(item.GetName()));
    }
}