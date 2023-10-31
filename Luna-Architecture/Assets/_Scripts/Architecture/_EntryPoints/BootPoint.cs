using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaleLuna.Architecture
{
    public class BootPoint : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _nextScene = 1;

        [SerializeField] private GameObject _dontDestroyObject;

        [SerializeField] private Test test;
        
        private void OnValidate()
        {
            _nextScene = Mathf.Clamp(_nextScene, 0, SceneManager.sceneCount);
        }

        private IEnumerator Start()
        {
            DontDestroyOnLoad(_dontDestroyObject);

            yield return null;

            ServiceLocator serviceLocator = _dontDestroyObject.AddComponent<ServiceLocator>();
            GameController gameController = _dontDestroyObject.AddComponent<GameController>();

            yield return new WaitForEndOfFrame();

            serviceLocator.Registarion<GameController>(gameController);

            SetupGameController();

            gameController.stateHolder.ChangeState<PlayState>();

            yield return new WaitForEndOfFrame();
            
        }

        private void SetupGameController()
        {
            GameController gameController = ServiceLocator.Instance.Get<GameController>();

            gameController.stateHolder
                .Registarion(new StartState(gameController));
            gameController.stateHolder
                .Registarion(new PlayState(gameController));
            gameController.stateHolder
                .Registarion(new PauseState(gameController));
            
            gameController.updatablesHolder.Registration(test);
        }

        private void JumpToScene(int sceneNum = -1)
        {
            if (sceneNum < 0) sceneNum = _nextScene;
            
            SceneManager.LoadScene(_nextScene);
        }
    }
}