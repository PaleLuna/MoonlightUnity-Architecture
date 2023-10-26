using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class UniqDataHolder<T> : IDataHolder<T>
{
    public UnityEvent<T> OnItemAdded { get; }
    
    private Dictionary<Type, T> _itemsMap = new Dictionary<Type, T>();


    public TP Registration<TP>(TP item, int order) where TP : T
    {
        throw new NotImplementedException();
    }

    public TP Registration<TP>(TP item) where TP : T
    {
        Type type = item.GetType();

        if (_itemsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists");

        _itemsMap[type] = item;

        return (TP)item;
    }

    public TP Unregistration<TP>(TP item) where TP : T
    {
        Type type = typeof(TP);

        if (!_itemsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        TP findedItem = (TP)_itemsMap[type];
        _ = _itemsMap.Remove(type);
        return item;
    }

    public T At(int index)
    {
        List<Type> types = _itemsMap.Keys.ToList();

        return _itemsMap[types[index]];
    }

    public TP GetFirstByType<TP>() where TP : T
    {
        Type type = typeof(TP);
        
        return (TP)_itemsMap[type];
    }

    public void ForEach(Action<T> action)
    {
        List<T> items = _itemsMap.Values.ToList();
        
        items.ForEach(action);
    }
}