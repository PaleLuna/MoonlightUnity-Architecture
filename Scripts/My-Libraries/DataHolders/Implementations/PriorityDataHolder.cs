using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

namespace PaleLuna.DataHolder
{
public class PriorityDataHolder<T> : IDataHolder<T>
{
    #region Fields
    private const int DEFAULT_CAPACITY = 10;

    private UnityEvent<T> _itemAddedEvent;

    private List<ItemPriorityPackage<T>> _items;
    
    public UnityEvent<T> OnItemAdded => _itemAddedEvent;

    public int Count => _items.Count;
    #endregion

    #region [ Constructors ]
    public PriorityDataHolder(int capacity = DEFAULT_CAPACITY)
    {
        _items = new(capacity);
    }
    #endregion

    #region [ Registration Region ]
        public void Registration(PriorityDataHolder<T> items)
        {
            Merge(items);
        }
    
        public void Registration<TP>(TP item, int order) where TP : T
        {
            ItemPriorityPackage<T> itemPackage = new(item, order);

            if(Count != 0)
                InsertItem(itemPackage);
            else
                _items.Add(itemPackage);
        }
    
        public void Registration<TP>(TP item) where TP : T
        {
            _items.Add(new ItemPriorityPackage<T>(item));
        }
    
        public bool Unregistration<TP>(TP item) where TP : T
        {
            ItemPriorityPackage<T> itemPackage = FindPriorityPackage(item);

            _items.Remove(itemPackage);

            return itemPackage != null;
        }
    
        public TP Unregistration<TP>(int index) where TP : T
        {
            if(index < 0 || index >= Count) return default;

            TP item = (TP)_items[index].Unpack();
            _items.Remove(_items[index]);

            return item;
        }
    #endregion

    #region [ Access Methods ]
    public T At(int index)
    {
        if(index >= 0 && index < Count)
            return _items[index].Unpack();

        throw new IndexOutOfRangeException($"index {index} is not included in the boundaries of the list");
    }
    #endregion

    #region [ Methods of interaction ]
    public void Clear() => _items.Clear();
    
    public IDataHolder<T> Filter(Func<T, bool> predicate)
    {
        PriorityDataHolder<T> temp = new PriorityDataHolder<T>(Count);
    
            _items.ForEach(item =>
            {
                if (predicate(item.Unpack()))
                    temp.Registration(item.Unpack());
            });
    
            return temp;
    }
    
    public void ForEach(Action<T> action)
    {
        for(int i = 0; i < _items.Count; i++)
            action(_items[i].Unpack());
    }
    public void ForEach(Action<ItemPriorityPackage<T>> action)
    {
        for(int i = 0; i < _items.Count; i++)
            action(_items[i]);
    }
        
    public T[] ToArray()
    {
        T[] items = new T[Count];

        ForEach((T item) => items.Append(item));

        return items;
    }

        
    public override string ToString()
    {
        string result = "";

        ForEach((ItemPriorityPackage<T> item) => 
        {
            Debug.Log(item.Unpack());
            result += $"{item.Unpack()}: Priority - {item.priority}\n";
        });

        return result;
    }
    #endregion

    #region [ Private Methods ]
    private ItemPriorityPackage<T> FindPriorityPackage<TP>(TP item) where TP : T
    {
        for(int i = 0; i < _items.Count; i++)
        {
            if(_items[i].Unpack().Equals(item))
                return _items[i];
        }
        
        return null;
    }

    private void InsertItem(ItemPriorityPackage<T> item)
    {
        for(int i = 0; i < _items.Count; i++)
        {
            if(item.Compare(_items[i]) == 1)
                continue;

            _items.Insert(i, item);
            return;
        }

        _items.Add(item);

    }

    private void Merge(PriorityDataHolder<T> other)
    {
        List<ItemPriorityPackage<T>> newList = new(Count + other.Count);

        int i = 0;
        int j = 0;

        for(int k = 0; k < newList.Capacity; k++)
        {
            if(i >= Count)
                newList.Add(other._items[j++]);
            else if(j >= other.Count)
                newList.Add(_items[i++]);
            
            else if(_items[i].Compare(other._items[j]) == 1)
                newList.Add(other._items[j++]);
            else
                newList.Add(_items[i++]);
        }

        _items = newList;
    }
    #endregion

    ~PriorityDataHolder() => Clear();
}

public class ItemPriorityPackage<T>
{
    private readonly T _item;
    private int _priority;

    public int priority => _priority;

    public ItemPriorityPackage(T item)
    {
        _item = item;
        _priority = -1;
    }

    public ItemPriorityPackage(T item, int priority)
    {
        _item = item;
        SetPriority(priority);
    }

    public T Unpack() => _item;

    public void SetPriority(int priority) => _priority = priority > 0 ? priority : 1;

    public int Compare(ItemPriorityPackage<T> other)
    {
        if(priority == -1 || _priority > other.priority)
            return 1;
        if(_priority == other._priority)
            return 0;

        return -1;
    }
}
}
