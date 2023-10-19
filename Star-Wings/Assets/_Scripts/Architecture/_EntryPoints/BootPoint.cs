using System;
using System.Collections;
using _Scripts.Architecture.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootPoint : MonoBehaviour
{
    [SerializeField] private GameObject _managersHolder;

    private IEnumerator Start()
    {
        GameManager gameManager = _managersHolder.AddComponent<GameManager>();
        yield return null;
        gameManager.isExists = true;
        
        DontDestroyOnLoad(_managersHolder);

        SceneManager.LoadScene(1);
    }
}
