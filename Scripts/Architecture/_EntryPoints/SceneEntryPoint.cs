using PaleLuna.Patterns.State.Game;
using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Services;
using Services;
using UnityEngine;
using NaughtyAttributes;
using PaleLuna.Architecture.Loops;

namespace PaleLuna.Architecture.EntryPoint
{

    [AddComponentMenu("Moonlight Unity / Entry Points / Scene Boot")]
    public class SceneEntryPoint : EntryPoint
    {
        #pragma warning disable 414
        [Header("Start game state settings"), HorizontalLine(color: EColor.Green)]
        [SerializeField]
        private bool _setManualGameState = true;
        #pragma warning restore 414

        [SerializeField, ShowIf("_setManualGameState")]
        private GameStatesEnum _defaultGameState = GameStatesEnum.Play;
        [Space]

        protected ServiceLocator _localServices = new ServiceLocator();

        protected override async UniTask Setup()
        {
            CheckServiceManager();

            ServiceManager.Instance.LocalServices = _localServices;
            FillSceneLocator();

            await base.Setup();

            ProcessBaggage();
            SetGameState();
        }

        protected virtual void FillSceneLocator() { }

        protected virtual void ProcessBaggage() { }

        private void CheckServiceManager()
        {
            if (ServiceManager.Instance == null)
            {
                SceneLoaderService sceneLoader = new();

                Debug.LogWarning("ServiceManager is null. Reload");

                sceneLoader.LoadScene(0);
            }
        }

        protected void SetGameState()
        {
            switch (_defaultGameState)
            {
                case GameStatesEnum.Pause:
                    ServiceManager.Instance
                        .GlobalServices.Get<GameLoops>()
                        .stateHolder.ChangeState<PauseState>();
                    break;

                case GameStatesEnum.Play:
                    ServiceManager.Instance
                        .GlobalServices.Get<GameLoops>()
                        .stateHolder.ChangeState<PlayState>();
                    break;
            }
        }
    }
}
