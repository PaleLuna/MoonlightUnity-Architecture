using System;
using System.Collections.Generic;

public class ServiceLocator : Singletone<ServiceLocator>,  IServiceHolder
{
    private Dictionary<Type, Object> _componentsMap;

    public ServiceLocator()
    {
        _componentsMap = new Dictionary<Type, object>();
    }
    
    public TP Register<TP>(TP newComponent)
    {
        Type type = newComponent.GetType();

        if (_componentsMap.ContainsKey(type))
            throw new Exception($"Cannot add item of type {type}. This type already exists");

        _componentsMap[type] = newComponent;

        return newComponent;
    }

    public void Unregister<TP>()
    {
        Type type = typeof(TP);

        if (!_componentsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        _componentsMap.Remove(type);
    }

    public TP Get<TP>()
    {
        Type type = typeof(TP);

        if (!_componentsMap.ContainsKey(type))
            throw new Exception($"item of type {type} doesn't exist in this map");

        return (TP)_componentsMap[type];
    }
}