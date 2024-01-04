namespace PaleLuna.Architecture.Initializer
{
    /**
 * @brief Интерфейс IInitializer представляет собой контракт для классов, реализующих инициализацию.
 *
 * IInitializer определяет свойство status, предоставляющее текущий статус инициализации, и метод StartInit для начала инициализации.
 */
    public interface IInitializer
    {
        /**
         * @brief Получает текущий статус инициализации.
         */
        public InitStatus status { get; }

        /**
         * @brief Метод, запускающий инициализацию.
         */
        public void StartInit();
    }
    
    /**
     * @brief Перечисление InitStatus определяет возможные статусы инициализации.
     */
    public enum InitStatus
    {
        /**
         * @brief Инициализация еще не начата.
         */
        Shutdown,
        /**
         * @brief Идет процесс инициализации.
         */
        Initialization,
        /**
         * @brief Инициализация завершена.
         */
        Done
    }
}

