using System;
using UnityEngine;

public interface ITypeCounter<T>
{
    public void AddItem<TP>(TP item, int count = 1) where TP : T;

    public TP Pick<TP>() where TP : T;
    public TP PopItems<TP>(int count = 1) where TP : T;

    public int CheckCount<TP>() where TP : T;

    public void RemoveEmpty();

    public void ForEach(Action<T> action);
    public void ForEach(Action<ItemHolder<T>> action);
}