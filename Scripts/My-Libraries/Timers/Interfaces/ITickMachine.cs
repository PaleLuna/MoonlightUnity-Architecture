using UnityEngine.Events;

namespace PaleLuna.Timers
{
public interface ITickMachine
{
    public TimerStatus timerStatus {get;}


    public void Start();
    public void Stop();

    public ITickMachine SetTimeForTick(float time);
    public ITickMachine SetAction(UnityAction action);
}
}