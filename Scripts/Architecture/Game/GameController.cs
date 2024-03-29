using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Services;
using PaleLuna.DataHolder;
using PaleLuna.DataHolder.Updatables;
using PaleLuna.Patterns.State;
using PaleLuna.Patterns.State.Game;
using UnityEngine;

namespace PaleLuna.Architecture.Controllers
{
    /**
     * @brief Класс GameController управляет различными аспектами игры.
     * 
     * GameController является главным контроллером игры, ответственным за управление обновлением, паузой и состояниями игры.
     * Реализует интерфейсы IService и IStartable, что позволяет его использование в качестве сервиса и компонента, который может быть инициализирован при старте.
     */
    public class GameController : MonoBehaviour, IService, IStartable
    {
        /**
        * @brief Флаг, указывающий, был ли GameController успешно инициализирован.
        */
        private bool _isStarted;

        /**
        * @brief Хранилище компонентов, поддерживающих паузу.
        */
        public DataHolder<IPausable> pausablesHolder { get; private set; }

        /**
         * @brief Хранилище компонентов, поддерживающих старт.
         */
        public DataHolder<IStartable> startableHolder { get; private set; }

        /**
        * @brief Объект для хранения и управления обновляемыми компонентами.
        */
        public UpdatablesHolder updatablesHolder { get; private set; }

        /**
         * @brief Хранилище состояний игры.
         */
        public StateHolder<GameStateBase> stateHolder { get; private set; }

        /**
         * @brief Получает значение, указывающее, был ли GameController успешно инициализирован.
         */
        public bool IsStarted => _isStarted;

        /**
         * @brief Метод, вызываемый при старте GameController.
         * 
         * Метод OnStart инициализирует GameController, создавая необходимые хранилища и объекты для управления состоянием игры.
         */
        public void OnStart()
        {
            if (_isStarted) return;

            stateHolder = new StateHolder<GameStateBase>();

            updatablesHolder = new UpdatablesHolder();
            pausablesHolder = new DataHolder<IPausable>();
            startableHolder = new DataHolder<IStartable>();

            _isStarted = true;
        }

        #region Registration
        public void Registration(IUpdatable component) 
        {
            updatablesHolder.Registration(component);
        }
        public void Registration(IFixedUpdatable component)
        {
            updatablesHolder.Registration(component);
        }
        public void Registration(ILateUpdatable component)
        {
            updatablesHolder.Registration(component);
        }
        public void Unregistration(IUpdatable component) 
        {
            updatablesHolder.UnRegistration(component);
        }
        public void Unregistration(IFixedUpdatable component)
        {
            updatablesHolder.UnRegistration(component);
        }
        public void Unregistration(ILateUpdatable component)
        {
            updatablesHolder.UnRegistration(component);
        }

        #endregion

        #region MonoEvents

        /**
         * @brief Метод, вызываемый в каждом кадре для обновления обновляемых компонентов.
         */
        private void Update()
        {
            updatablesHolder.everyFrameUpdatablesHolder
                .ForEach(updatable => updatable.EveryFrameRun());
        }

        /**
         * @brief Метод, вызываемый в каждом FixedUpdate для обновления компонентов, требующих фиксированную частоту обновления.
         */
        private void FixedUpdate()
        {
            updatablesHolder.fixedUpdatablesHolder
                .ForEach(updatable => updatable.FixedFrameRun());
        }

        /**
         * @brief Метод, вызываемый в каждом LateUpdate для обновления компонентов, требующих обновление после Update.
         */
        private void LateUpdate()
        {
            updatablesHolder.lateUpdatablesHolder
                .ForEach(updatable => updatable.LateUpdateRun());
        }

        /**
         * @brief Метод, вызываемый в каждом такте игрового времени для обновления компонентов с использованием метода EveryTickRun.
         */
        private void Tick()
        {
            updatablesHolder.tickUpdatableHolder
                .ForEach(updatable => updatable.EveryTickRun());
        }

        #endregion
    }
}