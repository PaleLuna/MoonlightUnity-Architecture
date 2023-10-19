using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootPoint : MonoBehaviour
{
    private const int NEXT_SCENE = 1;

    [SerializeField] private GameObject _managersHolder;

    private IEnumerator Start()
    {
        GameManager gameManager = _managersHolder.AddComponent<GameManager>();
        yield return null;
        gameManager.isExists = true;

        DontDestroyOnLoad(_managersHolder);

        SceneManager.LoadScene(NEXT_SCENE);
    }
}