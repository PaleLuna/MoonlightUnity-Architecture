using System;
using UnityEngine;

namespace PaleLuna.DataHolder.Dictionary
{
    [Serializable]
    public class SerializedDictionary<T>
    {
        [SerializeField]
        private SerializedDictionaryItem<T>[] _items; 

        public DictionaryHolder<string, T> Convert()
        {
            DictionaryHolder<string, T> dictionaryHolder = new DictionaryHolder<string, T>();

            for(int i = 0; i < _items.Length; i++)
                dictionaryHolder.Registration(_items[i].key, _items[i].item);

            return dictionaryHolder;
        }
    }

    [Serializable]
    public class SerializedDictionaryItem<T>
    {
        [SerializeField]
        public string key;

        [SerializeField]
        public T item;
    }
}
