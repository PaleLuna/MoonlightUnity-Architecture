using PaleLuna.Architecture.GameComponent;

namespace PaleLuna.DataHolder.Updatables
{
    /**
     * @brief Хранилище для регистрации и отмены регистрации различных типов обновляемых объектов.
     *
     * UpdatablesHolder предоставляет отдельные хранилища для объектов, которые должны обновляться каждый кадр, в фиксированный кадр, после всех остальных обновлений и с использованием таймера.
     */
    public class UpdatablesHolder
    {
        /** @brief Хранилище для объектов, обновляемых каждый кадр. */
        public DataHolder<IUpdatable> everyFrameUpdatablesHolder { get; private set; }

        /** @brief Хранилище для объектов, обновляемых в фиксированный кадр. */
        public DataHolder<IFixedUpdatable> fixedUpdatablesHolder { get; private set; }

        /** @brief Хранилище для объектов, обновляемых после всех остальных обновлений. */
        public DataHolder<ILateUpdatable> lateUpdatablesHolder { get; private set; }

        /** @brief Хранилище для объектов, обновляемых с использованием таймера. */
        public DataHolder<ITickUpdatable> tickUpdatableHolder { get; private set; }

        /**
         * @brief Конструктор класса.
         *
         * Создает экземпляр класса и инициализирует хранилища для каждого типа обновляемых объектов.
         */
        public UpdatablesHolder()
        {
            everyFrameUpdatablesHolder = new DataHolder<IUpdatable>();
            fixedUpdatablesHolder = new DataHolder<IFixedUpdatable>();
            lateUpdatablesHolder = new DataHolder<ILateUpdatable>();
            tickUpdatableHolder = new DataHolder<ITickUpdatable>();
        }

        #region Registration

        /**
        * @brief Регистрация объекта, обновляемого каждый кадр.
        *
        * @param item Объект для регистрации.
        *
        * Пример использования:
        * @code
        * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
        * updatablesHolder.Registration(new MyUpdatableObject());
        * @endcode
        */
        public void Registration(IUpdatable item) => everyFrameUpdatablesHolder.Registration(item);

        /**
        * @brief Регистрация объекта, обновляемого в фиксированный кадр.
        *
        * @param item Объект для регистрации.
        *
        * Пример использования:
        * @code
        * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
        * updatablesHolder.Registration(new MyFixedUpdatableObject());
        * @endcode
        */
        public void Registration(IFixedUpdatable item) => fixedUpdatablesHolder.Registration(item);

        /**
        * @brief Регистрация объекта, обновляемого после всех остальных обновлений.
        *
        * @param item Объект для регистрации.
        *
        * Пример использования:
        * @code
        * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
        * updatablesHolder.Registration(new MyLateUpdatableObject());
        * @endcode
        */
        public void Registration(ILateUpdatable item) => lateUpdatablesHolder.Registration(item);

        /**
         * @brief Регистрация объекта, обновляемого с использованием таймера.
         *
         * @param item Объект для регистрации.
         *
         * Пример использования:
         * @code
         * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
         * updatablesHolder.Registration(new MyTickUpdatableObject());
         * @endcode
         */
        public void Registration(ITickUpdatable item) => tickUpdatableHolder.Registration(item);

        #endregion

        #region Unregistartion

        /**
         * @brief Отмена регистрации объекта, обновляемого каждый кадр.
         *
         * @param item Объект для отмены регистрации.
         *
         * Пример использования:
         * @code
         * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
         * updatablesHolder.UnRegistration(myEveryFrameUpdatableObject);
         * @endcode
         */
        public void UnRegistration(IUpdatable item) =>
            everyFrameUpdatablesHolder.Unregistration(item);

        /**
        * @brief Отмена регистрации объекта, обновляемого в фиксированный кадр.
        *
        * @param item Объект для отмены регистрации.
        *
        * Пример использования:
        * @code
        * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
        * updatablesHolder.UnRegistration(myFixedUpdatableObject);
        * @endcode
        */
        public void UnRegistration(IFixedUpdatable item) =>
            fixedUpdatablesHolder.Unregistration(item);

        /**
         * @brief Отмена регистрации объекта, обновляемого после всех остальных обновлений.
         *
         * @param item Объект для отмены регистрации.
         *
         * Пример использования:
         * @code
         * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
         * updatablesHolder.UnRegistration(myLateUpdatableObject);
         * @endcode
         */
        public void UnRegistration(ILateUpdatable item) =>
            lateUpdatablesHolder.Unregistration(item);

        /**
         * @brief Отмена регистрации объекта, обновляемого с использованием таймера.
         *
         * @param item Объект для отмены регистрации.
         *
         * Пример использования:
         * @code
         * UpdatablesHolder updatablesHolder = new UpdatablesHolder();
         * updatablesHolder.UnRegistration(myTickUpdatableObject);
         * @endcode
         */
        public void UnRegistration(ITickUpdatable item) => tickUpdatableHolder.Unregistration(item);

        #endregion
    }
}
