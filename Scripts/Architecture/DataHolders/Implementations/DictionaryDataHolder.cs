using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PaleLuna.DataHolder
{
    /**
     * @brief Класс для хранения уникальных элементов с использованием словаря.
     *
     * Этот класс реализует интерфейс IUniqDataHolder<T> и использует словарь для хранения уникальных элементов по их типу.
     * @tparam T Тип элементов, хранящихся в словаре.
     */
    public class DictionaryDataHolder<T> : IUniqDataHolder<T>
    {
        /** @brief Событие, вызываемое при добавлении нового элемента. */
        private readonly UnityEvent<T> _onItemAdded = new();

        /** @brief Словарь для хранения уникальных элементов по их типу. */
        private readonly Dictionary<Type, T> _itemsMap;

        /** @brief Свойство для доступа к событию добавления элемента. */
        public UnityEvent<T> OnItemAdded => _onItemAdded;

        /** @brief Количество уникальных элементов в словаре. */
        public int Count => _itemsMap.Count;

        /**
         * @brief Конструктор класса.
         *
         * Создает экземпляр класса с пустым словарем.
         */
        public DictionaryDataHolder() => _itemsMap = new();

        /**
         * @brief Регистрация уникального элемента в словаре по его типу.
         *
         * @tparam TP Тип уникального элемента для регистрации.
         * @param item Уникальный элемент для регистрации.
         * @return Зарегистрированный уникальный элемент.
         *
         * Пример использования:
         * @code
         * DictionaryDataHolder<SomeBaseClass> dataHolder = new DictionaryDataHolder<SomeBaseClass>();
         * SomeDerivedClass derivedItem = new SomeDerivedClass();
         * dataHolder.Registration(derivedItem);
         * @endcode
         */
        public TP Registration<TP>(TP item)
            where TP : T
        {
            Type type = item.GetType();

            if (_itemsMap.ContainsKey(type))
                throw new Exception($"Cannot add item of type {type}. This type already exists");

            _itemsMap[type] = item;

            _onItemAdded.Invoke(item);
            return (TP)item;
        }

        /**
         * @brief Отмена регистрации уникального элемента из словаря по его типу.
         *
         * @tparam TP Тип уникального элемента для отмены регистрации.
         * @return Отмененный уникальный элемент.
         *
         * Пример использования:
         * @code
         * DictionaryDataHolder<SomeBaseClass> dataHolder = new DictionaryDataHolder<SomeBaseClass>();
         * SomeDerivedClass removedItem = dataHolder.Unregistration<SomeDerivedClass>();
         * @endcode
         */
        public TP Unregistration<TP>()
            where TP : T
        {
            Type type = typeof(TP);

            if (!_itemsMap.ContainsKey(type))
                throw new Exception($"item of type {type} doesn't exist in this map");

            TP findedItem = (TP)_itemsMap[type];
            _ = _itemsMap.Remove(type);
            return findedItem;
        }

        /**
        * @brief Получение уникального элемента по его типу.
        *
        * @tparam TP Тип уникального элемента для поиска.
        * @return Уникальный элемент указанного типа.
        *
        * Пример использования:
        * @code
        * DictionaryDataHolder<SomeBaseClass> dataHolder = new DictionaryDataHolder<SomeBaseClass>();
        * SomeDerivedClass retrievedItem = dataHolder.GetByType<SomeDerivedClass>();
        * @endcode
        */
        public TP GetByType<TP>()
            where TP : T
        {
            Type type = typeof(TP);
            return (TP)_itemsMap[type];
        }

        /**
         * @brief Применение действия ко всем уникальным элементам в словаре.
         *
         * @param action Действие, применяемое к каждому уникальному элементу в словаре.
         *
         * Пример использования:
         * @code
         * DictionaryDataHolder<SomeBaseClass> dataHolder = new DictionaryDataHolder<SomeBaseClass>();
         * dataHolder.ForEach(item => Debug.Log(item));
         * @endcode
         */
        public void ForEach(Action<T> action)
        {
            foreach (T item in _itemsMap.Values)
                action(item);
        }

        /**
         * @brief Деструктор класса.
         *
         * Очищает словарь при уничтожении объекта.
         */
        ~DictionaryDataHolder() => _itemsMap.Clear();
    }
}
