using System;
using System.Collections.Generic;

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
        Type key = GetKey<TP>();
        return _itemMap[key].Count;
    }
    public TP Pick<TP>() where TP : T
    {
        ItemHolder<TP> itemHolder = PickHolder<TP>();

        if (itemHolder == null) return default;

        return itemHolder.item;
    }
    public ItemHolder<TP> PickHolder<TP>() where TP : T
    {
        ItemHolder<T> itemHolder = GetItemHolder<TP>();

        if (itemHolder.Count <= 0) {
            RemoveEmpty();
            return default;
        }

        ItemHolder<TP> otherHolder = new ItemHolder<TP>(
            (TP)itemHolder.item, 
            itemHolder.Count);

        return otherHolder;
    }

    public TP PopItems<TP>(int count = 1) where TP : T
    {
        ItemHolder<T> itemHolder = GetItemHolder<TP>();

        itemHolder.Count -= count;

        return (TP)itemHolder.item;
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
            if (_itemMap[key].Count == 0)
                _itemMap.Remove(key);
    }

    private ItemHolder<T> GetItemHolder<TP>() where TP : T
    {
        Type key = GetKey<TP>();
        return _itemMap[key];
    }

    private Type GetKey<TP>() where TP : T
    {
        Type key = typeof(TP);
        CheckItem(key);

        return key;
    }

    private void CheckItem(Type key)
    {
        if (!_itemMap.ContainsKey(key))
            throw new NullReferenceException($"Item of type \"{key}\" doesn't exist in the dictionary");
    }

    public override string ToString()
    {
        string res = new string("");
        ForEach((ItemHolder<T> item) => res += $"{item.item.GetType()} : {item.Count}\n");

        return res;
    }

    public void ForEach(Action<T> action)
    {
        List<ItemHolder<T>> items = new List<ItemHolder<T>>(_itemMap.Values);

        items.ForEach(item => action.Invoke(item.item));
    }
    public void ForEach(Action<ItemHolder<T>> action)
    {
        List<ItemHolder<T>> items = new List<ItemHolder<T>>(_itemMap.Values);

        items.ForEach(item => action.Invoke(item));
    }
}

public class ItemHolder<T>
{
    public T item;
    private int count;

    public int Count { 
        get => count;
        set => count = value < 0 ? 0 : value;
    }

    public ItemHolder(T item, int count)
    {
        this.item = item;
        this.count = count;
    }
}