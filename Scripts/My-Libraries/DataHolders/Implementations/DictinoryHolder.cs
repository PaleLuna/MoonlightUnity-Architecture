using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PaleLuna.DataHolder.Dictinory
{
    /**
     * @brief Обобщенный класс-контейнер для словаря, реализующий интерфейс IDictionaryDataHolder.
     *
     * @tparam TKey Тип ключей словаря.
     * @tparam T Тип значений словаря.
     */
    public class DictinoryHolder<TKey, T> : IDictionaryDataHolder<TKey, T>
    {
        #region Properties
        private UnityEvent<T> _onItemAdded = new();

        private Dictionary<TKey, T> _dictinory;

        /**
         * @brief Доступ к событию добавления элемента.
         */
        public UnityEvent<T> OnItemAdded => _onItemAdded;

        /**
         * @brief Количество элементов в словаре.
         */
        public int Count => _dictinory.Count;
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
        public DictinoryHolder()
        {
            _dictinory = new();
        }
        #endregion

        #region Registation Methods

        #region Registation
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
                _dictinory[key] = item;
                _onItemAdded?.Invoke(item);
            }
            return this;
        }

        #endregion
        #region Unregistation
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

            _dictinory.Remove(key);

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

            TP item = (TP)_dictinory[key];
            _dictinory.Remove(key);

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
            TP item = (TP)_dictinory[key];

            if (item == null)
                _dictinory.Remove(key);

            return item;
        }

        /**
         * @brief Индексатор для доступа и установки значений в словаре.
         *
         * @param key Ключ элемента для доступа или установки.
         * @return Возвращает значение, ассоциированное с ключом.
         */
        public T this[TKey key]
        {
            get => _dictinory[key];
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
            foreach (T item in _dictinory.Values)
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

            foreach (T item in _dictinory.Values)
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
