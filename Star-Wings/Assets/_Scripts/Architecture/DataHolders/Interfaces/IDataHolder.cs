using System;
using UnityEngine.Events;

public interface IDataHolder<T> : IRegistration<T>
{
    UnityEvent<T> OnItemAdded { get; }
    T Registration(T item, int order);
    
    T At(int index);
    void ForEach(Action<T> action);
}