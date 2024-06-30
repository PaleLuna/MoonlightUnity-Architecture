using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace PaleLuna.DataHolder.Dictionary
{
    /**
     * @brief Обобщенный класс-контейнер для словаря, реализующий интерфейс IDictionaryDataHolder.
     *
     * @tparam TKey Тип ключей словаря.
     * @tparam T Тип значений словаря.
     */
    public class DictionaryHolder<TKey, T> : IDictionaryDataHolder<TKey, T>
    {
        #region Properties
        private UnityEvent<T> _onItemAdded = new();

        private Dictionary<TKey, T> _dictionary;

        /**
         * @brief Доступ к событию добавления элемента.
         */
        public UnityEvent<T> OnItemAdded => _onItemAdded;

        /**
         * @brief Количество элементов в словаре.
         */
        public int Count => _dictionary.Count;
        #endregion

        #region Constructors
        /**
         * @brief Конструктор по умолчанию, инициализирующий внутренний словарь.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * @endcode
         */
        public DictionaryHolder()
        {
            _dictionary = new();
        }
        #endregion

        #region Registration Methods

        #region Registration
        /**
         * @brief Регистрирует элемент с заданным ключом в словаре.
         *
         * @tparam TP Тип регистрируемого элемента.
         * @param key Ключ для регистрации элемента.
         * @param item Элемент, который требуется зарегистрировать.
         * @return Возвращает текущий экземпляр контейнера словаря.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * dictionary.Registration(1, "Значение1");
         * @endcode
         */
        public IDictionaryDataHolder<TKey, T> Registration<TP>(TKey key, TP item)
            where TP : T
        {
            if (item != null)
            {
                _dictionary[key] = item;
                _onItemAdded?.Invoke(item);
            }
            return this;
        }

        #endregion
        #region Unregistration
        /**
         * @brief Отменяет регистрацию элемента с заданным ключом из словаря.
         *
         * @param key Ключ элемента для отмены регистрации.
         * @return Возвращает текущий экземпляр контейнера словаря.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * dictionary.Unregistration(1);
         * @endcode
         */
        public IDictionaryDataHolder<TKey, T> Unregistration(TKey key)
        {
            ThrowExpIfNoKey<T>(key);

            _dictionary.Remove(key);

            return this;
        }

        /**
         * @brief Удаляет и возвращает элемент типа TP из словаря с заданным ключом.
         *
         * @tparam TP Тип удаляемого и возвращаемого элемента.
         * @param key Ключ элемента для удаления.
         * @return Возвращает удаленный элемент типа TP.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * string removedItem = dictionary.Pop<string>(1);
         * @endcode
         */
        public TP Pop<TP>(TKey key)
            where TP : T
        {
            ThrowExpIfNoKey<TP>(key);

            TP item = (TP)_dictionary[key];
            _dictionary.Remove(key);

            return item;
        }
        #endregion

        #endregion

        #region Acces Methods
        /**
         * @brief Получает элемент типа TP из словаря с заданным ключом. 
         * Использует проверку, в случае неудачи выкидывает исключение KeyNotFoundException.
         * Если ключ найден, но не соответствует типу, то выкидывает исключение InvalidCastException.
         *
         * @tparam TP Тип получаемого элемента.
         * @param key Ключ элемента для получения.
         * @return Возвращает элемент типа TP, если найден, иначе значение по умолчанию для TP.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * string value = dictionary.Get<string>(1);
         * @endcode
         */
        public TP Get<TP>(TKey key)
            where TP : T
        {
            TP item = (TP)_dictionary[key];

            if (item == null)
                _dictionary.Remove(key);

            return item;
        }

            public TKey[] GetKeys() =>
                _dictionary.Keys.ToArray();
            
            public T[] GetValues() =>
                _dictionary.Values.ToArray();
        /**
         * @brief Индексатор для доступа и установки значений в словаре.
         *
         * @param key Ключ элемента для доступа или установки.
         * @return Возвращает значение, ассоциированное с ключом.
         */
        public T this[TKey key]
        {
            get => _dictionary[key];
            set => Registration(key, value);
        }
        #endregion

        /**
         * @brief Выполняет заданное действие для каждого элемента в словаре.
         *
         * @param action Действие для выполнения для каждого элемента.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * dictionary.ForEach(item => Console.WriteLine(item));
         * @endcode
         */
        public void ForEach(Action<T> action)
        {
            foreach (T item in _dictionary.Values)
                action(item);
        }

        /**
         * @brief Фильтрует элементы в словаре на основе заданного условия.
         *
         * @param func Условие для фильтрации элементов.
         * @return Возвращает список отфильтрованных элементов.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * List<string> filteredList = dictionary.Filter(item => item.Length > 5);
         * @endcode
         */
        public List<T> Filter(Func<T, bool> func)
        {
            List<T> list = new();

            foreach (T item in _dictionary.Values)
                if (func(item))
                    list.Add(item);

            return list;
        }

        /**
         * @brief Проверяет, содержит ли словарь заданный ключ.
         *
         * @param key Ключ для проверки наличия.
         * @return Возвращает true, если ключ существует в словаре, иначе false.
         *
         * Пример использования:
         * @code
         * DictinoryHolder<int, string> dictionary = new DictinoryHolder<int, string>();
         * bool containsKey = dictionary.ContainsKey(1);
         * @endcode
         */
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        private void ThrowExpIfNoKey<TP>(TKey key) where TP : T
        {
            if (!ContainsKey(key))
                throw new KeyNotFoundException(
                    $"item of type {key.ToString()} doesn't exist in this map"
                );

            if(!(_dictionary[key] is TP))
                throw new InvalidCastException($"The key {key.ToString()} does not contain a type value {typeof(TP)}");
        }

        public override string ToString()
        {
            string dictString = "";
            
            foreach(TKey key in _dictionary.Keys)
                dictString += $"{key}: {_dictionary[key]}\n";

            return dictString;
        }
    }
}
