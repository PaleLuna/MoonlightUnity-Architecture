using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PaleLuna.Architecture
{
    public class GameControllerIInitializer : IInitializer
    {
        private InitStatus _status = InitStatus.Shutdown;

        private GameController _gameController;
        private GameObject _parent;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public InitStatus status => _status;

        public GameControllerIInitializer(GameObject parent)
        {
            _parent = parent;
        }
        
        public void StartInit()
        {
            if(_status != InitStatus.Shutdown) return;
            

            _status = InitStatus.Initialization;

            Init(_tokenSource.Token);
        }

        private async UniTaskVoid Init(CancellationToken token)
        {
            SetupGameController();
            await UniTask.Yield(cancellationToken: token);
            SetupStates();
            await UniTask.Yield(cancellationToken: token);

            ServiceLocator.Instance.Registarion(_gameController);

            _status = InitStatus.Done;
        }

        private void SetupGameController()
        {
            _gameController = _parent.AddComponent<GameController>();
            _gameController.OnStart();
        }
        
        private void SetupStates()
        {
            _gameController.stateHolder
                .Registarion(new StartState(_gameController));
            _gameController.stateHolder
                .Registarion(new PlayState(_gameController));
            _gameController.stateHolder
                .Registarion(new PauseState(_gameController));
        }

        ~GameControllerIInitializer() =>
            _tokenSource.Cancel();
    }
}