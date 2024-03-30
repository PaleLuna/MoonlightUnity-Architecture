using PaleLuna.Architecture.GameComponent;
using UnityEngine.Events;

public interface ITimer : IPausable
{
    public TimerStatus timerStatus {get;}

    public void Start();
    public void Stop();
    public void Reset();

    public void SetTime(float time);
    public void SetAction(UnityAction action);
}

public enum TimerStatus 
{
    Stop,
    Pause,
    Run
}
