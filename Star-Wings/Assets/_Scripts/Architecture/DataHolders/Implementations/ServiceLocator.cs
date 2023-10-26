using System;
using System.Collections.Generic;

public class ServiceLocator : Singletone<ServiceLocator>
{
    private UniqDataHolder<Object> _componentsMap;

    public ServiceLocator()
    {
        _componentsMap = new UniqDataHolder<object>();
    }
    
    public TP Registarion<TP>(TP item)
    {
        _componentsMap.Registration<TP>(item);

        return item;
    }
    
    public TP Unregistration<TP>(TP item)
    {
        return _componentsMap.Unregistration<TP>(item);
    }

    public TP Get<TP>()
    {
        return _componentsMap.GetFirstByType<TP>();
    }
}