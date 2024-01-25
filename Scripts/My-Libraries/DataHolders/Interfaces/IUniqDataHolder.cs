using System;
using UnityEngine.Events;

namespace PaleLuna.DataHolder
{
    /**
 * @brief Интерфейс для хранения уникальных элементов.
 *
 * Этот интерфейс предоставляет методы для регистрации, удаления и доступа к уникальным элементам.
 * @tparam T Тип элемента, который хранится в коллекции.
 */
    public interface IUniqDataHolder<T>
    {
        /** @brief Событие, вызываемое при добавлении нового уникального элемента. */
        UnityEvent<T> OnItemAdded { get; }

        /** @brief Количество уникальных элементов в коллекции. */
        int Count { get; }

        /**
    * @brief Регистрация уникального элемента в коллекции.
    *
    * @tparam TP Тип уникального элемента для регистрации.
    * @param item Уникальный элемент для регистрации.
    * @return Зарегистрированный уникальный элемент.
    */
        TP Registration<TP>(TP item) where TP : T;

        /**
     * @brief Отмена регистрации уникального элемента из коллекции.
     *
     * @tparam TP Тип уникального элемента для отмены регистрации.
     * @return Отмененный уникальный элемент.
     */
        TP Unregistration<TP>() where TP : T;

        /**
    * @brief Получение уникального элемента по типу.
    *
    * @tparam TP Тип уникального элемента для поиска.
    * @return Уникальный элемент указанного типа или null, если не найден.
    */
        TP GetByType<TP>() where TP : T;

        /**
 * @brief Применение действия ко всем уникальным элементам коллекции.
 *
 * @param action Действие, применяемое к каждому уникальному элементу коллекции.
 *
 * Пример использования:
 * @code
 * UniqDataHolder<int> dataHolder = new UniqDataHolder<int>();

 * // Регистрация уникальных элементов
 * dataHolder.Registration(1);
 * dataHolder.Registration(2);
 * dataHolder.Registration(3);

 * // Вывести каждый уникальный элемент на консоль
 * dataHolder.ForEach(item => Console.WriteLine(item));
 * @endcode
 */
        void ForEach(Action<T> action);
    }
}