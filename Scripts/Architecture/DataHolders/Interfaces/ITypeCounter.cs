using System;

namespace PaleLuna.DataHolder.Counter
{
/**
 * @brief Интерфейс для счетчика элементов определенного типа.
 *
 * Этот интерфейс предоставляет методы для учета, извлечения и манипулирования элементами определенного типа.
 * @tparam T Тип элементов, с которыми работает счетчик.
 */
public interface ITypeCounter<T>
{
    /**
    * @brief Метод для добавления элемента в счетчик с указанным количеством.
    *
    * @tparam TP Тип элемента для добавления.
    * @param item Элемент для добавления.
    * @param count Количество элементов для добавления (по умолчанию 1).
    */
    public void AddItem<TP>(TP item, int count = 1) where TP : T;

    /**
     * @brief Метод для извлечения элемента определенного типа из счетчика.
     *
     * @tparam TP Тип элемента для извлечения.
     * @return Извлеченный элемент.
     */
    public TP Pick<TP>() where TP : T;
    /**
   * @brief Метод для извлечения определенного количества элементов определенного типа из счетчика.
   *
   * @tparam TP Тип элемента для извлечения.
   * @param count Количество элементов для извлечения (по умолчанию 1).
   * @return Массив извлеченных элементов.
   */
    public TP PopItems<TP>(int count = 1) where TP : T;

    /**
    * @brief Метод для проверки количества элементов определенного типа в счетчике.
    *
    * @tparam TP Тип элемента для проверки.
    * @return Количество элементов указанного типа в счетчике.
    */
    public int CheckCount<TP>() where TP : T;

    /** @brief Метод для удаления элементов с нулевым количеством. */
    public void RemoveEmpty();

    /**
 * @brief Применение действия ко всем элементам счетчика.
 *
 * @param action Действие, применяемое к каждому элементу счетчика.
 *
 * Пример использования:
 * @code
 * TypeCounter<int> counter = new TypeCounter<int>();
 * counter.AddItem(1);
 * counter.AddItem(2);
 *
 * // Вывести каждый элемент на консоль
 * counter.ForEach(item => Console.WriteLine(item));
 * @endcode
 */
    public void ForEach(Action<T> action);
    
    /**
 * @brief Применение действия ко всем хранилищам элементов счетчика.
 *
 * @param action Действие, применяемое к каждому хранилищу элементов счетчика.
 *
 * Пример использования:
 * @code
 * TypeCounter<int> counter = new TypeCounter<int>();
 * counter.AddItem(1);
 * counter.AddItem(2, 3); // Добавим 3 элемента со значением 2
 *
 * // Вывести информацию о каждом хранилище на консоль
 * counter.ForEach(holder => Console.WriteLine($"Item: {holder.Item}, Count: {holder.Count}"));
 * @endcode
 */
    public void ForEach(Action<ItemHolder<T>> action);
}
}