using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PaleLuna.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture
{
    public class BootPoint : MonoBehaviour
    {
        #region Properties

        private const int DEFAULT_LIST_CAPACITY = 10;
        
        [Header("Next scene params")]
        [SerializeField, Min(0)] private int _nextScene = 1;

        [Header("Startables")] [SerializeReference, RequireInterface(typeof(IStartable))]
        private List<MonoBehaviour> _startables;

        private GameObject _dontDestroyObject;
        private List<IInitializer> _initializersList = new List<IInitializer>(DEFAULT_LIST_CAPACITY);

        #endregion

        #region Mono methods

        private void OnValidate() => 
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCount);

        private void Start() => 
            BootGame();
        
        #endregion
        
        private async UniTaskVoid BootGame()
        {
            _dontDestroyObject = new GameObject("DontDestroy");
            
            await UniTask.Yield();

            ServiceLocator serviceLocator = _dontDestroyObject.AddComponent<ServiceLocator>();

            FillInitializers();
            StartAllInitializers();
            
            int currentDoneInits = 0;
            
            while (currentDoneInits < _initializersList.Count)
            {
                int lastDoneInits = 0;
                
                foreach (IInitializer item in _initializersList)
                    if (item.status == InitStatus.Done)
                        lastDoneInits++;

                if (lastDoneInits > currentDoneInits)
                {
                    currentDoneInits = lastDoneInits;
                    print($"Loading services: {currentDoneInits} / {_initializersList.Count}");
                }

                await UniTask.Yield();
            }

            await UniTask.Yield();
            
            ServiceLocator.Instance.
                GetComponent<GameController>()
                .stateHolder
                .ChangeState<PlayState>();
            
            StartAllComponents();
        }

        #region Auxiliary methods

        private void FillInitializers() =>
            _initializersList
                .Add(new GameControllerIInitializer(_dontDestroyObject));

        private void StartAllInitializers() => 
            _initializersList.ForEach(initializer => initializer.StartInit());

        private void StartAllComponents() => 
            _startables.ForEach(behaviour => ((IStartable)behaviour).OnStart());

        private void JumpToScene(int sceneNum = -1) =>
            SceneManager.LoadScene(
                sceneNum < 0 ? _nextScene : sceneNum);

        #endregion
    }
}