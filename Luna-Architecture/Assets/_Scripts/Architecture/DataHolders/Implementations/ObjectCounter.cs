using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCounter<T> : ITypeCounter<T>
{
    private Dictionary<Type, ItemHolder<T>> _itemMap;

    public void AddItem<TP>(TP item, int count = 1) where TP : T
    {
        if (count <= 0) return;

        Type itemType = item.GetType();

        if(_itemMap.ContainsKey(itemType))
            AddToItem(itemType, count);
        else
            CreateItemHolder(itemType, item, count);
    }

    public int CheckCount<TP>() where TP : T
    {
        return _itemMap[typeof(TP)].count;
    }

    public TP PopItems<TP>(int count) where TP : T
    {
        ItemHolder<T> itemHolder = _itemMap[typeof(TP)];

        itemHolder.count -= count;

        return (TP)itemHolder.val;
    }

    private void AddToItem(Type key, int count)
    {
        ItemHolder<T> objectHolder = _itemMap[key];
        objectHolder.count += count;
    }
    private void CreateItemHolder<TP>(Type key, TP item, int count) where TP : T
    {
        _itemMap[key] = new ItemHolder<T>(item, count);
    }
}

struct ItemHolder<T>
{
    public T val;

    public int count { 
        get => count;
        set => count = value < 0 ? 0 : value;
    }

    public ItemHolder(T val, int count)
    {
        this.val = val;
        this.count = count;
    }
}
