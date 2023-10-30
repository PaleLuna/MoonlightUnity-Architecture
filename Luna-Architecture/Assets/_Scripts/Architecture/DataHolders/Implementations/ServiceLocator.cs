using System;
using System.Collections.Generic;

public class ServiceLocator : Singletone<ServiceLocator>
{
    private DictionaryDataHolder<Object> _componentsMap;

    public ServiceLocator()
    {
        _componentsMap = new DictionaryDataHolder<object>();
    }
    
    public TP Registarion<TP>(TP item)
    {
        return _componentsMap.Registration<TP>(item);
    }
    
    public TP Unregistration<TP>()
    {
        return _componentsMap.Unregistration<TP>();
    }

    public TP Get<TP>()
    {
        return _componentsMap.GetByType<TP>();
    }
}