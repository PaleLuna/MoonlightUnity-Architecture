using System;
using UnityEngine.Events;

public interface IDataHolder<T>
{
    UnityEvent<T> OnItemAdded { get; }

    T Registration(T item, int order);
    T Registration(T item);
    
    void UnRegistration(T item);

    T At(int index);

    void ForEach(Action<T> action);
}