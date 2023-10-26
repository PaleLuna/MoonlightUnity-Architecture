using System;
using UnityEngine.Events;

public interface IDataHolder<T>
{
    UnityEvent<T> OnItemAdded { get; }
    TP Registration<TP>(TP item, int order) where TP : T; 
    TP Registration<TP>(TP item) where TP : T;
    TP Unregistration<TP>(TP item) where TP : T;
    
    T At(int index);
    TP GetFirstByType<TP>() where TP : T;
    
    void ForEach(Action<T> action);
}