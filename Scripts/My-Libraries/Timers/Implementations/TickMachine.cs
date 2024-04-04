using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace PaleLuna.Timers.Implementations
{
public class TickMachine : ITickMachine
{
    private float _secondsForTick = 0;
    private UnityAction _action;

    private CancellationTokenSource _cancellationTokenSource;

    private TimerStatus _timerStatus = TimerStatus.Shutdown;
    public TimerStatus timerStatus => _timerStatus;


    public TickMachine(){}
    public TickMachine(float time, UnityAction action)
    {
        SetTimeForTick(time);
        SetAction(action);
    }


    public void Start()
    {
        StartMachine();
    }
    
    public void Stop()
    {
        StopMachine();
    }

    public ITickMachine SetTimeForTick(float time)
    {
        _secondsForTick = time;
        return this;
    }

    public ITickMachine SetAction(UnityAction action)
    {
        _action = action;
        return this;
    }

    private void StartMachine()
    {
        if(_timerStatus == TimerStatus.Run) return;

        _cancellationTokenSource = new CancellationTokenSource();
        _ = TickKeeper();

        _timerStatus = TimerStatus.Run;
    }
    private void StopMachine()
    {
        if(_timerStatus == TimerStatus.Shutdown) return;

        _cancellationTokenSource.Cancel();

        _timerStatus = TimerStatus.Shutdown;
    }

    private async UniTask TickKeeper()
    {
        while(true)
        {
            if(_secondsForTick == 0)
                await UniTask.Yield(_cancellationTokenSource.Token);
            else
                await UniTask.WaitForSeconds(_secondsForTick, ignoreTimeScale: true, cancellationToken: _cancellationTokenSource.Token);

            _action.Invoke();
        }
    }
}
}

