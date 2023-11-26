using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture
{
    [AddComponentMenu("Moonlight Unity / Entry Points / Game Boot")]
    public class BootPoint : EntryPoint
    {
        #region Properties

        [Header("Next scene params")]
        [SerializeField, Min(0)] private int _nextScene = 1;

        private GameObject _dontDestroyObject;

        #endregion

        #region Mono methods

        private void OnValidate() => 
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCount);

        private void Start() =>
           _ = Setup();
        
        #endregion
        
        protected override async UniTaskVoid Setup()
        {
            _dontDestroyObject = new GameObject("DontDestroy");
            DontDestroyOnLoad(_dontDestroyObject);
            
            await UniTask.Yield();

            _ = _dontDestroyObject.AddComponent<ServiceLocator>();

            FillInitializers();
            StartAllInitializers();

            await LoadAllServices();

            await UniTask.Yield();
            
            ServiceLocator.Instance.
                GetComponent<GameController>()
                .stateHolder
                .ChangeState<PlayState>();

            CompileAllComponents();
            StartAllComponents();
            
            JumpToScene();
        }
        
        #region Auxiliary methods

        protected override void FillInitializers() =>
            _initializersList
                .Add(new GameControllerIInitializer(_dontDestroyObject));

        protected override void StartAllInitializers() => 
            _initializersList.ForEach(initializer => initializer.StartInit());

        private void JumpToScene(int sceneNum = -1) =>
            SceneManager.LoadScene(
                sceneNum < 0 ? _nextScene : sceneNum);

        #endregion
    }
}