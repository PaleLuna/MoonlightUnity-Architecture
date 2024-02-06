using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PaleLuna.DataHolder.Dictinory
{
    public class DictinoryHolder<TKey, T> : IDictionaryDataHolder<TKey, T>
    {
        #region Properties
        private UnityEvent<T> _onItemAdded = new();

        private Dictionary<TKey, T> _dictinory;

        public UnityEvent<T> OnItemAdded => _onItemAdded;

        public int Count => throw new NotImplementedException();
        #endregion

        #region Constructors
        public DictinoryHolder()
        {
            _dictinory = new();
        }
        #endregion

        #region Registation Methods

        #region Registation
        public IDictionaryDataHolder<TKey, T> Registration<TP>(TKey key, TP item)
            where TP : T
        {
            if (item != null)
            {
                _dictinory[key] = item;
                _onItemAdded?.Invoke(item);
            }
            return this;
        }

        #endregion
        #region Unregistation
        public IDictionaryDataHolder<TKey, T> Unregistration(TKey key)
        {
            ThrowExpIfNoKey<T>(key);

            _dictinory.Remove(key);

            return this;
        }

        public TP Pop<TP>(TKey key)
            where TP : T
        {
            ThrowExpIfNoKey<TP>(key);

            TP item = (TP)_dictinory[key];
            _dictinory.Remove(key);

            return item;
        }
        #endregion

        #endregion

        #region Acces Methods
        public TP Get<TP>(TKey key)
            where TP : T
        {
            


            TP item = (TP)_dictinory[key];

            if (item == null)
                _dictinory.Remove(key);

            return item;
        }

        //TODO
        public T this[TKey key]
        {
            get => _dictinory[key];
            set => Registration(key, value);
        }

        #endregion


        public void ForEach(Action<T> action)
        {
            foreach (T item in _dictinory.Values)
                action(item);
        }

        public List<T> Filter(Func<T, bool> func)
        {
            List<T> list = new();

            foreach (T item in _dictinory.Values)
                if (func(item))
                    list.Add(item);

            return list;
        }

        public bool ContainsKey(TKey key) => _dictinory.ContainsKey(key);

        private void ThrowExpIfNoKey<TP>(TKey key) where TP : T
        {
            if (!ContainsKey(key))
                throw new KeyNotFoundException(
                    $"item of type {key.ToString()} doesn't exist in this map"
                );

            if(!(_dictinory[key] is TP))
                throw new InvalidCastException($"The key {key.ToString()} does not contain a type value {typeof(TP)}");
        }
    }
}
