using System.Threading;
using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Controllers;
using PaleLuna.Patterns.State.Game;
using Services;
using UnityEngine;

namespace PaleLuna.Architecture.Initializer
{
    /**
 * @brief Инициализатор для GameController и его состояний.
 *
 * GameControllerInitializer реализует интерфейс IInitializer и предназначен для инициализации компонента GameController
 * и его состояний при старте игры.
 */
    public class GameControllerInitializer : IInitializer
    {
        /**
         * @brief Текущий статус инициализации.
         */
        private InitStatus _status = InitStatus.Shutdown;

        /**
         * @brief Объект GameController, который будет инициализирован.
         */
        private GameController _gameController;

        /**
       * @brief Родительский объект, к которому будет добавлен GameController.
       */
        private GameObject _parent;

        /**
         * @brief Токен отмены для асинхронных операций.
         */
        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        /**
         * @brief Получает текущий статус инициализации.
         */
        public InitStatus status => _status;

        /**
        * @brief Конструктор инициализатора GameControllerInitializer.
        *
        * @param parent Родительский объект, к которому будет добавлен GameController.
        */
        public GameControllerInitializer(GameObject parent)
        {
            _parent = parent;
        }

        /**
         * @brief Метод, запускающий инициализацию GameController.
         *
         * Если текущий статус не Shutdown, инициализация не выполняется.
         */
        public void StartInit()
        {
            if (_status != InitStatus.Shutdown)
                return;

            _status = InitStatus.Initialization;

            _ = Init(_tokenSource.Token);
        }

        /**
         * @brief Асинхронный метод инициализации GameController.
         *
         * Инициализирует GameController, устанавливает состояния и регистрирует GameController в ServiceLocator.
         */
        private async UniTaskVoid Init(CancellationToken token)
        {
            SetupGameController();
            await UniTask.Yield(cancellationToken: token);
            SetupStates();
            await UniTask.Yield(cancellationToken: token);

            ServiceManager.Instance.GlobalServices.Registarion(_gameController);

            _status = InitStatus.Done;
        }

        /**
         * @brief Метод устанавливает GameController на объекте _parent.
         */
        private void SetupGameController()
        {
            _gameController = _parent.AddComponent<GameController>();
            _gameController.OnStart();
        }

        /**
         * @brief Метод устанавливает состояния в stateHolder GameController.
         */
        private void SetupStates()
        {
            _gameController.stateHolder.Registarion(new StartState(_gameController));
            _gameController.stateHolder.Registarion(new PlayState(_gameController));
            _gameController.stateHolder.Registarion(new PauseState(_gameController));
        }

        /**
         * @brief Деструктор, который отменяет токен при уничтожении объекта.
         */
        ~GameControllerInitializer() => _tokenSource.Cancel();
    }
}
