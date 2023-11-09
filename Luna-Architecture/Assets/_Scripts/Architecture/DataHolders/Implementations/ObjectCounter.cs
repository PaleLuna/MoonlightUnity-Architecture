using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCounter<T> : ITypeCounter<T>
{
    private Dictionary<Type, ItemHolder<T>> _itemMap;

    public ObjectCounter()
    {
        _itemMap = new();
    }

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
        Type key = typeof(TP);
        CheckItem(key);
        return _itemMap[key].Count;
    }

    public TP PopItems<TP>(int count = 1) where TP : T
    {
        Type key = typeof(TP);
        CheckItem(key);

        ItemHolder<T> itemHolder = _itemMap[key];

        itemHolder.Count -= count;

        return (TP)itemHolder.val;
    }

    private void AddToItem(Type key, int count)
    {
        ItemHolder<T> objectHolder = _itemMap[key];
        objectHolder.Count += count;
    }
    private void CreateItemHolder<TP>(Type key, TP item, int count) where TP : T
    {
        _itemMap[key] = new ItemHolder<T>(item, count);
    }

    public void RemoveEmpty()
    {
        List<Type> keys = new List<Type>(_itemMap.Keys);

        foreach (Type key in keys)
        {
            if (_itemMap[key].Count == 0)
                _itemMap.Remove(key);
        }
    }

    public override string ToString()
    {
        string res = new string("");
        List<ItemHolder<T>> itemHolders = new List<ItemHolder<T>>(_itemMap.Values);

        foreach (ItemHolder<T> item in itemHolders)
        {
            res += $"{item.val.GetType()} : {item.Count}\n";
        }

        return res;
    }

    private void CheckItem(Type key)
    {
        if (!_itemMap.ContainsKey(key))
            throw new NullReferenceException($"Item of type \"{key}\" doesn't exist in the dictionary");
    }
}

class ItemHolder<T>
{
    public T val;
    private int count;

    public int Count { 
        get => count;
        set => count = value < 0 ? 0 : value;
    }

    public ItemHolder(T val, int count)
    {
        this.val = val;
        this.count = count;
    }
}
