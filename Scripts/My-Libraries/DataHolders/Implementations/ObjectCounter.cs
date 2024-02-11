using System;
using System.Collections.Generic;

namespace PaleLuna.DataHolder.Counter
{
    /**
     * @brief Класс для подсчета и управления количеством объектов различных типов.
     *
     * Этот класс реализует интерфейс ITypeCounter<T> и предоставляет функционал по подсчету и управлению количеством объектов различных типов.
     * @tparam T Тип объектов, подсчитываемых этим классом.
     */

    public class ObjectCounter<T> : ITypeCounter<T>
    {
        /** @brief Словарь для хранения объектов и их количества по типу. */
        private Dictionary<Type, ItemHolder<T>> _itemMap;

        /**
        * @brief Конструктор класса.
        *
        * Создает экземпляр класса с пустым словарем.
        */
        public ObjectCounter()
        {
            _itemMap = new();
        }

        /**
        * @brief Добавление объекта в подсчет с указанным количеством.
        *
        * @tparam TP Тип добавляемого объекта.
        * @param item Добавляемый объект.
        * @param count Количество добавляемых объектов.
        *
        * Пример использования:
        * @code
        * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
        * SomeDerivedClass derivedItem = new SomeDerivedClass();
        * objectCounter.AddItem(derivedItem, 3);
        * @endcode
        */
        public void AddItem<TP>(TP item, int count = 1)
            where TP : T
        {
            if (count <= 0)
                return;

            Type itemType = item.GetType();

            if (_itemMap.ContainsKey(itemType))
                AddToItem(itemType, count);
            else
                CreateItemHolder(itemType, item, count);
        }

        /**
        * @brief Проверка количества объектов указанного типа.
        *
        * @tparam TP Тип объектов.
        * @return Количество объектов указанного типа.
        *
        * Пример использования:
        * @code
        * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
        * int count = objectCounter.CheckCount<SomeDerivedClass>();
        * @endcode
        */
        public int CheckCount<TP>()
            where TP : T
        {
            Type key = GetKey<TP>();
            return _itemMap[key].Count;
        }

        public int CheckCount(Type type)
        {
            CheckItem(type);

            return _itemMap[type].Count;
        }

        /**
        * @brief Получение одного объекта указанного типа.
        *
        * @tparam TP Тип объектов.
        * @return Один объект указанного типа или значение по умолчанию, если объектов нет.
        *
        * Пример использования:
        * @code
        * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
        * SomeDerivedClass retrievedItem = objectCounter.Pick<SomeDerivedClass>();
        * @endcode
        */
        public TP Pick<TP>()
            where TP : T
        {
            ItemHolder<TP> itemHolder = PickHolder<TP>();

            if (itemHolder == null)
                return default;

            return itemHolder.item;
        }

        /**
        * @brief Получение объекта-хранителя указанного типа.
        *
        * @tparam TP Тип объектов.
        * @return Объект-хранитель указанного типа или значение по умолчанию, если объектов нет.
        *
        * Пример использования:
        * @code
        * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
        * ItemHolder<SomeDerivedClass> holder = objectCounter.PickHolder<SomeDerivedClass>();
        * @endcode
        */
        public ItemHolder<TP> PickHolder<TP>()
            where TP : T
        {
            ItemHolder<T> itemHolder = GetItemHolder<TP>();

            if (itemHolder.Count <= 0)
            {
                RemoveEmpty();
                return default;
            }

            ItemHolder<TP> otherHolder = new ItemHolder<TP>((TP)itemHolder.item, itemHolder.Count);

            return otherHolder;
        }

        /**
         * @brief Уменьшение количества объектов указанного типа на указанное количество.
         *
         * @tparam TP Тип объектов.
         * @param count Количество объектов для удаления.
         * @return Один объект указанного типа или значение по умолчанию, если объектов нет.
         *
         * Пример использования:
         * @code
         * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
         * SomeDerivedClass removedItem = objectCounter.PopItems<SomeDerivedClass>(2);
         * @endcode
         */
        public TP PopItems<TP>(int count = 1)
            where TP : T
        {
            ItemHolder<T> itemHolder = GetItemHolder<TP>();

            itemHolder.Count -= count;

            return (TP)itemHolder.item;
        }

        /** @brief Добавление к количеству объектов указанного типа. */
        private void AddToItem(Type key, int count)
        {
            ItemHolder<T> objectHolder = _itemMap[key];
            objectHolder.Count += count;
        }

        /** @brief Создание объекта-хранителя для указанного типа. */
        private void CreateItemHolder<TP>(Type key, TP item, int count)
            where TP : T
        {
            _itemMap[key] = new ItemHolder<T>(item, count);
        }

        /** @brief Удаление объектов с нулевым количеством. */
        public void RemoveEmpty()
        {
            List<Type> keys = new List<Type>(_itemMap.Keys);

            foreach (Type key in keys)
                if (_itemMap[key].Count == 0)
                    _itemMap.Remove(key);
        }

        /** @brief Получение объекта-хранителя указанного типа. */
        private ItemHolder<T> GetItemHolder<TP>()
            where TP : T
        {
            Type key = GetKey<TP>();
            return _itemMap[key];
        }

        /** @brief Получение ключа для указанного типа. */
        private Type GetKey<TP>()
            where TP : T
        {
            Type key = typeof(TP);
            CheckItem(key);

            return key;
        }

        /** @brief Проверка наличия объекта указанного типа. */
        private void CheckItem(Type key)
        {
            if (!_itemMap.ContainsKey(key))
                throw new NullReferenceException(
                    $"Item of type \"{key}\" doesn't exist in the dictionary"
                );
        }

        public override string ToString()
        {
            string res = new string("");
            ForEach((ItemHolder<T> item) => res += $"{item.item.GetType()} : {item.Count}\n");

            return res;
        }

        /**
        * @brief Применение действия ко всем объектам в счетчике.
        *
        * @param action Действие, применяемое к каждому объекту в счетчике.
        *
        * Пример использования:
        * @code
        * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
        * objectCounter.ForEach(item => Debug.Log(item));
        * @endcode
        */
        public void ForEach(Action<T> action)
        {
            List<ItemHolder<T>> items = new List<ItemHolder<T>>(_itemMap.Values);

            items.ForEach(item => action.Invoke(item.item));
        }

        /**
         * @brief Применение действия ко всем объектам-хранителям в счетчике.
         *
         * @param action Действие, применяемое к каждому объекту-хранителю в счетчике.
         *
         * Пример использования:
         * @code
         * ObjectCounter<SomeBaseClass> objectCounter = new ObjectCounter<SomeBaseClass>();
         * objectCounter.ForEach(item => Debug.Log(item));
         * @endcode
         */
        public void ForEach(Action<ItemHolder<T>> action)
        {
            List<ItemHolder<T>> items = new List<ItemHolder<T>>(_itemMap.Values);

            items.ForEach(item => action.Invoke(item));
        }
    }

    /**
     * @brief Класс-хранитель объекта и его количества.
     *
     * Этот класс используется для хранения объекта и количества, связанного с этим объектом.
     * @tparam T Тип объекта, хранящегося в объекте-хранителе.
     */
    public class ItemHolder<T>
    {
        /** @brief Объект, хранящийся в объекте-хранителе. */
        public T item;

        /** @brief Количество объектов, хранящихся в объекте-хранителе. */
        private int count;

        /**
        * @brief Свойство для доступа к количеству объектов.
        *
        * Геттер возвращает текущее количество объектов, а сеттер устанавливает новое значение, но не допускает отрицательных значений.
        */
        public int Count
        {
            get => count;
            set => count = value < 0 ? 0 : value;
        }

        /**
         * @brief Конструктор объекта-хранителя.
         *
         * @param item Объект для хранения в объекте-хранителе.
         * @param count Количество объектов для хранения в объекте-хранителе.
         */
        public ItemHolder(T item, int count)
        {
            this.item = item;
            this.count = count;
        }
    }
}
