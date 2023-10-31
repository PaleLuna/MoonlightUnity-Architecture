using System;
using System.Collections.Generic;
using UnityEngine.Events;

public interface IDataHolder<T>
{
    UnityEvent<T> OnItemAdded { get; }
    
    TP Registration<TP>(TP item, int order) where TP : T;
    public void Registration(List<T> items, ListRegistrationType registrationType = ListRegistrationType.Replace);

    TP Registration<TP>(TP item) where TP : T;
    TP Unregistration<TP>(TP item) where TP : T;
    
    T At(int index);

    public void Clear();

    void ForEach(Action<T> action);
}

public enum ListRegistrationType
{
    AddToEnd,
    AddToStart,
    Replace
}