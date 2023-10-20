using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BootPoint : MonoBehaviour
{
    private const int NEXT_SCENE = 1;

    [SerializeField] private GameObject _dontDestroyObject;

    private IEnumerator Start()
    {
        yield return null;

        DontDestroyOnLoad(_dontDestroyObject);

        SceneManager.LoadScene(NEXT_SCENE);
    }
}