using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

public class Timer : ITimer
{
    private float _originTimeStart = 0;
    private float _remainingTime = 0;
    private float _elapsedTime = 0;

    private float _startTime = 0;

    private UnityAction _action;
    private CancellationTokenSource _token;

    private TimerStatus _timerStatus = TimerStatus.Stop;
    public TimerStatus timerStatus => _timerStatus;

    public float elapsedTime => _elapsedTime;
    public float remainingTime => _remainingTime - _elapsedTime;

    public Timer()
    {
    }

    public Timer(float time, UnityAction action)
    {
        SetTime(time);
        SetAction(action);
    }

    public void Start()
    {
        if(_timerStatus == TimerStatus.Run) return;
        Reset();

        StartClock();
    }

    public void Stop()
    {
        _ = TryStopClock();

        _timerStatus = TimerStatus.Stop;
    }

    public void Reset()
    {
        _remainingTime = _originTimeStart;
        _elapsedTime = 0;

        _startTime = Time.time;
    }

    public void Restart()
    {
        _ = TryStopClock();
        
        Start();
    }

    public void OnResume()
    {
        if(_timerStatus == TimerStatus.Run) return;

        _remainingTime -= _elapsedTime;

        StartClock();
    }

    public void OnPause()
    {
        if(TryStopClock())
            _timerStatus = TimerStatus.Pause;
    }

    public ITimer SetTime(float time)
    {
        _originTimeStart = time;
        Reset();

        return this;
    }

    public ITimer SetAction(UnityAction action)
    {
        _action = action;

        return this;
    }

    private void StartClock()
    {
        _token = new CancellationTokenSource();

        _startTime = Time.time;
        _ = KeepCountdown();

        _timerStatus = TimerStatus.Run;
    }
    private bool TryStopClock()
    {
        if(_timerStatus != TimerStatus.Run) return false;

        _token.Cancel();
        return true;
    }

    private async UniTask KeepCountdown()
    {
        while(_remainingTime > _elapsedTime)
        {
            _elapsedTime = Time.time - _startTime;

            await UniTask.Yield(_token.Token); 
        }

        _action.Invoke();
        Stop();
    }
}
