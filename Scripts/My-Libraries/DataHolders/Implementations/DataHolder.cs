using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace PaleLuna.DataHolder
{
    /**
 * @brief Класс для хранения и управления коллекцией элементов.
 *
 * Этот класс реализует интерфейс IDataHolder<T> и предоставляет методы для регистрации, удаления и манипулирования элементами.
 * @tparam T Тип элементов, хранящихся в коллекции.
 */
    public class DataHolder<T> : IDataHolder<T>
    {
        #region Properties
        /** @brief Емкость по умолчанию для списка элементов. */
        private const int DEFAULT_CAPACITY = 10;

        /** @brief Событие, вызываемое при добавлении нового элемента. */
        private readonly UnityEvent<T> onItemAdded = new();

        /** @brief Свойство для доступа к событию добавления элемента. */
        public UnityEvent<T> OnItemAdded => OnItemAdded;

        /** @brief Список элементов. */
        private List<T> _itemsList;

        /** @brief Количество элементов в коллекции. */
        public int Count => _itemsList.Count;
        #endregion

        #region Constructors
        /**
        * @brief Конструктор класса.
        *
        * Создает экземпляр класса с указанной начальной емкостью (по умолчанию 10).
        * @param startCapacity Начальная емкость списка элементов.
        */
        public DataHolder(int startCapacity = 0)
        {
            if (startCapacity <= 0)
                _itemsList = new List<T>(DEFAULT_CAPACITY);
            else
                _itemsList = new List<T>(startCapacity);
        }

        /**
         * @brief Конструктор класса с использованием существующего списка элементов.
         *
         * @param list Существующий список элементов.
         */
        public DataHolder(List<T> list)
        {
            ReplaceList(list);
        }
        public DataHolder(T[] array)
        {
            ReplaceList(new List<T>(array));
        }
        #endregion

        #region Registration

        /**
         * @brief Регистрация списка элементов в коллекции.
         *
         * @param otherItems Список элементов для регистрации.
         * @param registrationType Тип регистрации (добавление в конец, в начало, замена, объединение в конец).
         *
         * Пример использования:
         * @code
         * DataHolder<int> dataHolder = new DataHolder<int>();
         * List<int> otherItems = new List<int> { 1, 2, 3 };
         * dataHolder.Registration(otherItems, ListRegistrationType.AddToEnd);
         * @endcode
         */
        public void Registration(
            List<T> otherItems,
            ListRegistrationType registrationType = ListRegistrationType.Replace
        )
        {
            switch (registrationType)
            {
                case ListRegistrationType.AddToEnd:
                    MergeToEnd(otherItems);
                    break;
                case ListRegistrationType.AddToStart:
                    MergeToStart(otherItems);
                    break;
                case ListRegistrationType.Replace:
                    ReplaceList(otherItems);
                    break;
                case ListRegistrationType.MergeToEndUnion:
                    MergeToEndUnion(otherItems);
                    break;
                default:
                    ReplaceList(otherItems);
                    break;
            }
        }

            /**
         * @brief Регистрация элемента в коллекции с указанным порядком.
         *
         * @tparam TP Тип элемента для регистрации.
         * @param item Элемент для регистрации.
         * @param order Порядок, в котором элемент будет добавлен в коллекцию.
         * @return Зарегистрированный элемент.
         *
         * Пример использования:
         * @code
         * DataHolder<string> dataHolder = new DataHolder<string>();
         * dataHolder.Registration("Example", 0);
         * @endcode
         */
        public TP Registration<TP>(TP item, int order)
            where TP : T
        {
            _itemsList.Insert(order, item);
            onItemAdded?.Invoke(item);

            return item;
        }

            /**
         * @brief Регистрация элемента в конец коллекции.
         *
         * @tparam TP Тип элемента для регистрации.
         * @param item Элемент для регистрации.
         * @return Зарегистрированный элемент.
         *
         * Пример использования:
         * @code
         * DataHolder<float> dataHolder = new DataHolder<float>();
         * dataHolder.Registration(3.14f);
         * @endcode
         */
        public TP Registration<TP>(TP item)
            where TP : T
        {
            _itemsList.Add(item);
            onItemAdded?.Invoke(item);
            return item;
        }

        /**
     * @brief Отмена регистрации элемента из коллекции.
     *
     * @tparam TP Тип элемента для отмены регистрации.
     * @param item Элемент для отмены регистрации.
     * @return Отмененный элемент.
     *
     * Пример использования:
     * @code
     * DataHolder<int> dataHolder = new DataHolder<int>();
     * int itemToRemove = 42;
     * dataHolder.Unregistration(itemToRemove);
     * @endcode
     */
        public TP Unregistration<TP>(TP item)
            where TP : T
        {
            _itemsList.Remove(item);
            return item;
        }
        public TP Unregistration<TP>(int index)
            where TP : T
        {
            TP item = (TP)_itemsList[index];
            _itemsList.RemoveAt(index);
            return item;
        }
        #endregion

        #region MergeListMethods

        /**
        * @brief Замена списка элементов в коллекции новым списком.
        *
        * @param otherList Новый список элементов для замены.
        */
        private void ReplaceList(List<T> otherList)
        {
            Clear();
            _itemsList = new(otherList);
        }

        /**
         * @brief Добавление всех элементов из другого списка в конец текущего списка.
         *
         * @param otherList Список элементов для добавления в конец текущего списка.
         */
        private void MergeToEnd(List<T> otherList)
        {
            if (otherList == null)
                return;

            _itemsList.AddRange(otherList);
        }

        /**
        * @brief Добавление всех элементов из текущего списка в начало другого списка.
        *
        * @param otherList Список элементов для добавления в начало текущего списка.
        */
        private void MergeToStart(List<T> otherList)
        {
            if (otherList == null)
                return;
            otherList.AddRange(_itemsList);

            ReplaceList(otherList);
        }

        /**
       * @brief Объединение элементов из другого списка в конец текущего списка, сохраняя уникальность.
       *
       * @param other Список элементов для объединения в конец текущего списка.
       */
        private void MergeToEndUnion(List<T> other)
        {
            _itemsList = _itemsList.Union(other).ToList();
        }

        #endregion

        #region Access Methods
        /**
     * @brief Получение элемента по индексу.
     *
     * @param index Индекс элемента в коллекции.
     * @return Элемент по указанному индексу.
     *
     * Пример использования:
     * @code
     * DataHolder<string> dataHolder = new DataHolder<string>();
     * string item = dataHolder.At(2);
     * @endcode
     */
        public T At(int index)
        {
            T res = default;

            if (index >= 0 && index < _itemsList.Count)
                res = _itemsList[index];

            if(res == null)
                Unregistration<T>(index);

            return res;
        }

        #region Indexing
        public T this[int index]
        {
            get => _itemsList[index];
            set => _itemsList[index] = value;
        }

        #endregion

        #endregion

        #region Methods of interaction
        public T[] ToArray()
        {
            return _itemsList.ToArray();
        }

        public void ForEach(Action<T> action) => _itemsList.ForEach(action);

        public IDataHolder<T> Filter(Func<T, bool> predicate)
        {
            DataHolder<T> temp = new DataHolder<T>(this.Count);

            _itemsList.ForEach(item =>
            {
                if (predicate(item))
                    temp.Registration(item);
            });

            return temp;
        }

        /**
* @brief Удаление всех нулевых элементов из коллекции.
*
* Пример использования:
* @code
* DataHolder<object> dataHolder = new DataHolder<object>();
* dataHolder.RemoveAllNulls();
* @endcode
*/
        public void RemoveAllNulls() => _itemsList.RemoveAll(item => item == null);

        public void Clear() => _itemsList?.Clear();
        #endregion

        ~DataHolder() => _itemsList.Clear();
        public override string ToString()
        {
            string res = "{ ";

            _itemsList.ForEach(item => res += $"{item.ToString()} ");

            res += "}";

            return res;
        }
    }
}
