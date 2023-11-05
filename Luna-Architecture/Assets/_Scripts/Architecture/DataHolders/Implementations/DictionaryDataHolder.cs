using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class DictionaryDataHolder<T> : IUniqDataHolder<T>
{
    private readonly UnityEvent<T> _onItemAdded = new();
    private readonly Dictionary<Type, T> _itemsMap;

    public UnityEvent<T> OnItemAdded => _onItemAdded;

    public int Count => _itemsMap.Count;

    public DictionaryDataHolder()
    {
        _itemsMap = new();
    }

    public TP Registration<TP>(TP item) where TP : T
    {
        Type type = item.GetType();

        if (_itemsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists");

        _itemsMap[type] = item;

        _onItemAdded.Invoke(item);
        return (TP)item;
    }

    public TP Unregistration<TP>() where TP : T
    {
        Type type = typeof(TP);
        
        if (!_itemsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        TP findedItem = (TP)_itemsMap[type];
        _ = _itemsMap.Remove(type);
        return findedItem;
    }

    public TP GetByType<TP>() where TP : T
    {
        Type type = typeof(TP);
        return (TP)_itemsMap[type];
    }

    public void ForEach(Action<T> action)
    {
        foreach (T item in _itemsMap.Values)
            action(item);
    }

    ~DictionaryDataHolder() => _itemsMap.Clear();
}