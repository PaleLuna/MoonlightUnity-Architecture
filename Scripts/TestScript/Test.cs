using System;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Counter;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Controllers;
using UnityEngine;

public class Test : MonoBehaviour, IUpdatable, IStartable
{
    [SerializeField] private string _name;
    private ObjectCounter<Item> _itemCounter;

    private bool _isStartable = false;

    public bool IsStarted => _isStartable;

    public void OnStart()
    {
        if (_isStartable) return;


        _isStartable = true;

        ServiceLocator.Instance.Get<GameController>().Registatrion(this);
    }
        

    public void EveryFrameRun() => 
        Debug.Log($"Run {this}");


    private void TestObjectCounter()
    {
        _itemCounter = new ObjectCounter<Item>();

        try
        {
            _itemCounter.PopItems<Apple>();
        }
        catch (NullReferenceException ex)
        {
            print(ex.Message);
        }

        //Тест на добавление
        _itemCounter.AddItem(new Apple(), 10);
        _itemCounter.AddItem(new Rock());
        _itemCounter.AddItem(new Stick(),3);


        print(_itemCounter);
        _itemCounter.ForEach(item => print(item.GetName()));

        //Тест на удаление
        _itemCounter.PopItems<Apple>(4);
        _itemCounter.PopItems<Stick>(5);
        _itemCounter.PopItems<Rock>(1);

        print(_itemCounter);

        _itemCounter.RemoveEmpty();
        print(_itemCounter);

        try
        {
            _itemCounter.CheckCount<Stick>();
        }
        catch (NullReferenceException ex)
        {
            print(ex.Message);
        }

        print(_itemCounter.PopItems<Apple>().EatApple());

        print(_itemCounter.Pick<Apple>().EatApple());

        print(_itemCounter.PickHolder<Apple>().item.EatApple());

        try
        {
            print(_itemCounter.PopItems<Stick>());
        }
        catch (NullReferenceException ex)
        {
            print(ex.Message);
        }

        try
        {
            print(_itemCounter.Pick<Stick>());
        }
        catch (NullReferenceException ex)
        {
            print(ex.Message);
        }

        try
        {
            print(_itemCounter.PickHolder<Stick>().Count);
        }
        catch (NullReferenceException ex)
        {
            print(ex.Message);
        }

    }

    private void OnDestroy() =>
        ServiceLocator.Instance?
            .Get<GameController>()
                .updatablesHolder?.UnRegistration(this);
}