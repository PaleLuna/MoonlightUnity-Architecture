using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class DataHolder<T> : IDataHolder<T>
{
    private const int DEFAULT_CAPACITY = 10;

    private UnityEvent<T> onItemAdded = new UnityEvent<T>();
    public UnityEvent<T> OnItemAdded => OnItemAdded;

    
    private List<T> _itemsList;

    public DataHolder(int startCapacity = 0)
    {
        if (startCapacity <= 0)
            _itemsList = new List<T>(DEFAULT_CAPACITY);
        else
            _itemsList = new List<T>(startCapacity);
    }


    public TP Registration<TP>(TP item, int order) where TP : T
    {
        _itemsList.Insert(order, item);
        
        onItemAdded?.Invoke(item);
        
        return item;
    }

    public TP Registration<TP>(TP item) where TP : T
    {
        _itemsList.Add(item);
        
        onItemAdded?.Invoke(item);
        
        return item;
    }

    public TP Unregistration<TP>(TP item) where TP : T
    {
        _itemsList.Remove(item);
        return item;
    }

    public T At(int index) => _itemsList[index];
    public TP GetFirstByType<TP>() where TP : T
    {
        throw new NotImplementedException();
    }

    public void ForEach(Action<T> action) => _itemsList.ForEach(action);
}