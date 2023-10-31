using UnityEngine;

public class Test : MonoBehaviour, IUpdatable
{
    public void EveryFrameRun()
    {
        Debug.Log($"Run {this}");
    }

    private void OnDestroy()
    {
        ServiceLocator.Instance.Get<GameController>().updatablesHolder.UnRegistration(this);
    }
}