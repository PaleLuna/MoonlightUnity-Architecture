using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture
{
    public class BootPoint : MonoBehaviour
    {
        private const int DEFAULT_LIST_CAPACITY = 10;
        
        [SerializeField, Min(0)] private int _nextScene = 1;
        [SerializeField] private Test test;
        private GameObject _dontDestroyObject;

        private List<IInitializer> _initializersList = new List<IInitializer>(DEFAULT_LIST_CAPACITY);
        
        
        private void OnValidate() => 
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCount);

        private void Start() => 
            BootGame();

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
        }

        private void FillInitializers()
        {
            _initializersList
                .Add(new GameControllerIInitializer(_dontDestroyObject));
        }

        private void StartAllInitializers()
        {
            _initializersList.ForEach(initializer => initializer.StartInit());
        }

        private void JumpToScene(int sceneNum = -1)
        {
            if (sceneNum < 0) sceneNum = _nextScene;
            
            SceneManager.LoadScene(_nextScene);
        }
    }
}