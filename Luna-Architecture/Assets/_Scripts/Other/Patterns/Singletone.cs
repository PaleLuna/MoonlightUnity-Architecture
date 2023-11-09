using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance => GetInstance();

    private static T GetInstance()
    {
        if (!_instance)
        {
            _instance = FindFirstObjectByType<T>();

            if (!_instance)
            {
                GameObject gObj = GameObject.Find("DontDestroy");
                _instance = gObj?.AddComponent<T>();
            }
        }

        return _instance;
    }

    private void Awake()
    {
        if (!_instance)
            _instance = this as T;
        else
            Destroy(this);
    }
}