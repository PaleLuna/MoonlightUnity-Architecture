using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = System.Object;

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
        
        serviceLocator.Registarion<GameController>(gameController);

        SetupGameController();
        
        gameController.stateHolder.ChangeState<PlayState>();
        Debug.Log(gameController.stateHolder.currentState);

        SceneManager.LoadScene(NEXT_SCENE);
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
    }
}