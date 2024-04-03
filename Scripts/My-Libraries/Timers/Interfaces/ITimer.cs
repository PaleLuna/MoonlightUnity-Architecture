using PaleLuna.Architecture.GameComponent;
using UnityEngine.Events;

public interface ITimer : IPausable
{
    public TimerStatus timerStatus {get;}
    public float remainingTime {get;}

    public void Start();
    public void Stop();
    public void Reset();
    public void Restart();

    public ITimer SetTime(float time);
    public ITimer SetAction(UnityAction action);
}

public enum TimerStatus 
{
    Stop,
    Pause,
    Run
}
