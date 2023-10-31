using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class DataHolder<T> : IDataHolder<T>
{
    private const int DEFAULT_CAPACITY = 10;

    private UnityEvent<T> onItemAdded = new UnityEvent<T>();
    public UnityEvent<T> OnItemAdded => OnItemAdded;

    private List<T> _itemsList;

    public DataHolder(int startCapacity = 0)
    {
        if (startCapacity <= 0)
            _itemsList = new List<T>(DEFAULT_CAPACITY);
        else
            _itemsList = new List<T>(startCapacity);
    }


    #region Registration

    public void Registration(List<T> otherItems, ListRegistrationType registrationType = ListRegistrationType.Replace)
    {
        switch (registrationType)
        {
            case ListRegistrationType.AddToEnd:
                MergeToEnd(otherItems);
                break;
            case ListRegistrationType.AddToStart:
                MergeToStart(otherItems);
                break;
            case ListRegistrationType.Replace:
                ReplaceList(otherItems);
                break;
            default:
                ReplaceList(otherItems);
                break;
        }
    }
    
    public TP Registration<TP>(TP item, int order) where TP : T
    {
        _itemsList.Insert(order, item);
        onItemAdded?.Invoke(item);
        
        return item;
    }

    

    public TP Registration<TP>(TP item) where TP : T
    {
        _itemsList.Add(item);
        onItemAdded?.Invoke(item);
        return item;
    }

    public TP Unregistration<TP>(TP item) where TP : T
    {
        _itemsList.Remove(item);
        return item;
    }
    #endregion

    public T At(int index)
    {
        RemoveAllNulls();
        
        T res = default(T);

        if (index >= 0 && index < _itemsList.Count)
            res = _itemsList[index];
        
        return res;
    }

    public void Clear() => _itemsList.Clear();

    public void ForEach(Action<T> action)
    {
        RemoveAllNulls();
        _itemsList.ForEach(action);
    }

    private void RemoveAllNulls() => 
        _itemsList.RemoveAll(item => item == null);
    
    
    #region MergeListMethods

    private void ReplaceList(List<T> otherList)
    {
        Clear();
        _itemsList = otherList;
    }

    private void MergeToEnd(List<T> otherList)
    {
        if(otherList == null) return;
        
        _itemsList.AddRange(otherList);
    }

    private void MergeToStart(List<T> otherList)
    {
        if(otherList == null) return;
        otherList.AddRange(_itemsList);
        
        ReplaceList(otherList);
    }

    #endregion
}