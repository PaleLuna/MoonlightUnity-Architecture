using System.Threading;
using Cysharp.Threading.Tasks;
using PaleLuna.Patterns.State.Game;
using Services;
using UnityEngine;
using PaleLuna.Architecture.Loops;

namespace PaleLuna.Architecture.Initializer
{
    /**
 * @brief Инициализатор для GameController и его состояний.
 *
 * GameControllerInitializer реализует интерфейс IInitializer и предназначен для инициализации компонента GameController
 * и его состояний при старте игры.
 */
    public class GameLoopsInitializer : InitializerBase
    {
        private GameLoops _gameLoops;

        private GameObject _parent;

        public GameLoopsInitializer(GameObject parent)
        {
            _parent = parent;
        }

        public override void StartInit()
        {
            if (_status != InitStatus.Shutdown)
                return;

            _status = InitStatus.Initialization;

            Init();
        }

        private void Init()
        {
            SetupGameController();
            SetupStates();

            ServiceManager.Instance.GlobalServices.Registarion(_gameLoops);

            _status = InitStatus.Done;
        }

        private void SetupGameController()
        {
            _gameLoops = _parent.AddComponent<GameLoops>();
            _gameLoops.OnStart();
        }

        private void SetupStates()
        {
            _gameLoops.stateHolder.Registarion(new PlayState(_gameLoops));
            _gameLoops.stateHolder.Registarion(new PauseState(_gameLoops));
        }
    }
}
