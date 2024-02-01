using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PaleLuna.DataHolder
{
    /**
    * @brief Интерфейс для хранения и управления коллекцией элементов.
    *
    * Этот интерфейс предоставляет методы для регистрации, удаления и обработки элементов в коллекции.
    * @tparam T Тип элемента, хранящегося в коллекции.
    */
    public interface IDataHolder<T>
    {
        /** @brief Событие, вызываемое при добавлении нового элемента. */
        UnityEvent<T> OnItemAdded { get; }

        /** @brief Количество элементов в коллекции. */
        int Count { get; }

        /**
          * @brief Регистрация элемента в коллекции с указанным порядком.
          *
          * @tparam TP Тип элемента для регистрации.
          * @param item Элемент для регистрации.
          * @param order Порядок, в котором элемент будет добавлен в коллекцию.
          * @return Зарегистрированный элемент.
          */
        TP Registration<TP>(TP item, int order)
            where TP : T;

        /**
           * @brief Регистрация списка элементов в коллекции.
           *
           * @param items Список элементов для регистрации.
           * @param registrationType Тип регистрации (добавление в конец, в начало, замена, объединение в конец).
           */
        public void Registration(
            List<T> items,
            ListRegistrationType registrationType = ListRegistrationType.Replace
        );

        /**
           * @brief Регистрация элемента в коллекции.
           *
           * @tparam TP Тип элемента для регистрации.
           * @param item Элемент для регистрации.
           * @return Зарегистрированный элемент.
           */
        TP Registration<TP>(TP item)
            where TP : T;

        /**
         * @brief Отмена регистрации элемента в коллекции.
         *
         * @tparam TP Тип элемента для отмены регистрации.
         * @param item Элемент для отмены регистрации.
         * @return Отмененный элемент.
         */
        TP Unregistration<TP>(TP item)
            where TP : T;
        TP Unregistration<TP>(int index) where TP : T;

        /**
           * @brief Получение элемента по индексу.
           *
           * @param index Индекс элемента в коллекции.
           * @return Элемент по указанному индексу.
           */
        T At(int index);

        IDataHolder<T> Filter(Func<T, bool> predicate);

        /** @brief Очистка коллекции. */
        public void Clear();

        /**
          * @brief Применение действия ко всем элементам коллекции.
          *
          * @param action Действие, применяемое к каждому элементу коллекции.
          */
        void ForEach(Action<T> action);

        /** @brief Преобразование коллекции в массив. */
        T[] ToArray();
    }

    /**
    * @brief Перечисление для типов регистрации элементов в коллекции.
    */
    public enum ListRegistrationType
    {
        AddToEnd,
        AddToStart,
        Replace,
        MergeToEndUnion
    }
}
