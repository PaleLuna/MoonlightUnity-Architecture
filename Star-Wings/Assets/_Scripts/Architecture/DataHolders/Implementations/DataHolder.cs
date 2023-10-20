using System;
using System.Collections.Generic;
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


    public T Registration(T item, int order)
    {
        _itemsList.Insert(order, item);
        
        onItemAdded?.Invoke(item);
        
        return item;
    }

    public T Registration(T item)
    {
        _itemsList.Add(item);
        
        onItemAdded?.Invoke(item);
        
        return item;
    }

    public void Unregistration(T item) => _itemsList.Remove(item);

    public T At(int index) => _itemsList[index];

    public void ForEach(Action<T> action) => _itemsList.ForEach(action);
}