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

        serviceLocator.Register<GameController>(gameController);

        SceneManager.LoadScene(NEXT_SCENE);
    }
}