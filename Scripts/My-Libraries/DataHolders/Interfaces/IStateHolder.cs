using PaleLuna.Patterns.State;

namespace PaleLuna.DataHolder
{
    /**
 * @brief Интерфейс для объекта, управляющего состоянием.
 *
 * Этот интерфейс определяет метод для изменения текущего состояния объекта.
 * @tparam T Тип состояния, который может быть изменен.
 */
    public interface IStateHolder<T> where T : State
    {
        /**
     * @brief Метод для изменения текущего состояния объекта.
     *
     * @tparam TP Новый тип состояния.
     */
        void ChangeState<TP>() where TP : T;
    }
}