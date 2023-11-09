using UnityEngine;

public class Test : MonoBehaviour, IUpdatable, IStartable
{
    public void OnStart() =>
        ServiceLocator.Instance.Get<GameController>()?
            .updatablesHolder.Registration(this);

    public void EveryFrameRun() => 
        Debug.Log($"Run {this}");

    private void OnDestroy() =>
        ServiceLocator.Instance
            .Get<GameController>()
                .updatablesHolder?.UnRegistration(this);
}