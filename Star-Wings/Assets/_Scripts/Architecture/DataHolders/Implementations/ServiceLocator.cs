using System;
using System.Collections.Generic;

public class ServiceLocator : Singletone<ServiceLocator>,  IServiceHolder
{
    private Dictionary<Type, Object> _componentsMap;

    public ServiceLocator()
    {
        _componentsMap = new Dictionary<Type, Object>();
    }
    
    public TP Registarion<TP>(TP item)
    {
        Type type = item.GetType();

        if (_componentsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists");

        _componentsMap[type] = item;

        return item;
    }
    
    public TP Unregistration<TP>()
    {
        Type type = typeof(TP);

        if (!_componentsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        TP item = (TP)_componentsMap[type];
        _componentsMap.Remove(type);

        return item;
    }

    public TP Get<TP>()
    {
        Type type = typeof(TP);

        if (!_componentsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        return (TP)_componentsMap[type];
    }
}