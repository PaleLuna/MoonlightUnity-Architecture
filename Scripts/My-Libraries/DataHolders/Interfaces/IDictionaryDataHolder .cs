using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PaleLuna.DataHolder
{
    public interface IDictionaryDataHolder<TKey, T>
    {
        UnityEvent<T> OnItemAdded { get; }

        int Count { get; }

        IDictionaryDataHolder<TKey, T> Registration<TP>(TKey key, TP item) where TP : T;

        IDictionaryDataHolder<TKey, T> Unregistration(TKey key);

        TP Get<TP>(TKey key) where TP : T;
        TP Pop<TP>(TKey key) where TP : T;

        void ForEach(Action<T> action);

    }
}