using Cysharp.Threading.Tasks;
using PaleLuna.Architecture.Loops;
using PaleLuna.Architecture.Initializer;
using PaleLuna.Architecture.Services;
using PaleLuna.Patterns.State.Game;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture.EntryPoint
{
    [AddComponentMenu("Moonlight Unity / Entry Points / Game Boot")]
    public class BootPoint : EntryPoint
    {
        #region Properties

        [Header("Next scene params")]
        [SerializeField, Min(0)]
        private int _nextScene = 1;

        private GameObject _dontDestroyObject;

        private ServiceLocator _globalServiceLocator = new ServiceLocator();
        #endregion

        #region Mono methods

        private void OnValidate() =>
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCountInBuildSettings - 1);

        #endregion

        protected override async UniTask Setup()
        {
            _dontDestroyObject = new GameObject("DontDestroy");
            DontDestroyOnLoad(_dontDestroyObject);

            _ = _dontDestroyObject.AddComponent<ServiceManager>();

            ServiceManager.Instance.GlobalServices = _globalServiceLocator;
            _globalServiceLocator.Registarion(new SceneLoaderService());

            await base.Setup();

            _globalServiceLocator.Get<GameLoops>().stateHolder.ChangeState<PauseState>();

            JumpToScene();
        }

        #region Auxiliary methods

        protected override void FillInitializers() =>
            _initializers.Registration(new GameLoopsInitializer(_dontDestroyObject));

        protected void JumpToScene(int sceneNum = -1)
        {
            SceneLoaderService sceneService = _globalServiceLocator.Get<SceneLoaderService>();

            sceneService.LoadScene(sceneNum < 0 ? _nextScene : sceneNum);
        }
        #endregion
    }
}
