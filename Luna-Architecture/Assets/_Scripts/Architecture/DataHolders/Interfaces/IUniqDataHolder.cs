using System;
using UnityEngine.Events;

public interface IUniqDataHolder<T>
{
    UnityEvent<T> OnItemAdded { get; }
    
    int Count { get; }
    
    TP Registration<TP>(TP item) where TP : T;
    TP Unregistration<TP>() where TP : T;
    
    TP GetByType<TP>() where TP : T;
    
    void ForEach(Action<T> action);
}