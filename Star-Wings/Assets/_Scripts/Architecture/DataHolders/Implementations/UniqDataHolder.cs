using System;
using System.Collections.Generic;

public class UniqDataHolder<T> : ISingleRegistration<T>
{
    private Dictionary<Type, T> _itemsMap;
    
    public TP Registarion<TP>(TP item) where TP : T
    {
        Type type = item.GetType();

        if (_itemsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists");

        _itemsMap[type] = item;

        return (TP)item;
    }

    public TP Unregistration<TP>() where TP : T
    {
        Type type = typeof(TP);

        if (!_itemsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        TP item = (TP)_itemsMap[type];
        _itemsMap.Remove(type);

        return item;
    }
}