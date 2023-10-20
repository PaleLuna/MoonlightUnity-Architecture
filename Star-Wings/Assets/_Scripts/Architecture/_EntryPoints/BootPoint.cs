using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BootPoint : MonoBehaviour
{
    private const int NEXT_SCENE = 1;

    [SerializeField] private GameObject _dontDestroyObject;

    private IEnumerator Start()
    {
        DontDestroyOnLoad(_dontDestroyObject);

        yield return null;

        ServiceLocator serviceLocator = _dontDestroyObject.AddComponent<ServiceLocator>();
        GameController gameController = _dontDestroyObject.AddComponent<GameController>();

        yield return new WaitForEndOfFrame();
        
        serviceLocator.Register<GameController>(gameController);

        SetupGameController();
        
        gameController.stateHolder.ChangeState<PlayState>();

        SceneManager.LoadScene(NEXT_SCENE);
    }

    private void SetupGameController()
    {
        GameController gameController = ServiceLocator.Instance.Get<GameController>();

        gameController.stateHolder
            .Register(new StartState(gameController));
        gameController.stateHolder
            .Register(new PlayState(gameController));
        gameController.stateHolder
            .Register(new PauseState(gameController));
    }
}