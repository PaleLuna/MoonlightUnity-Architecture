using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;

public class Timer : ITimer
{
    private float _originTimeStart = 0;
    private float _remainingTime = 0;
    private float _elapsedTime = 0;

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
        if(_timerStatus == TimerStatus.Run) _token.Cancel();
        Reset();

        StartClock();
    }

    public void OnResume()
    {
        if(_timerStatus == TimerStatus.Run) return;

        _remainingTime -= _elapsedTime;

        StartClock();
    }

    public void OnPause()
    {
        if(_timerStatus != TimerStatus.Run) return;

        _token.Cancel();
        _timerStatus = TimerStatus.Pause;
    }

    public void Stop()
    {
        OnPause();
        Reset();

        _timerStatus = TimerStatus.Stop;
    }

    public void Reset()
    {
        _remainingTime = _originTimeStart;
        _elapsedTime = 0;
    }

    public void SetTime(float time)
    {
        _originTimeStart = time;
    }

    public void SetAction(UnityAction action)
    {
        _action = action;
    }

    private void StartClock()
    {
        _token = new CancellationTokenSource();

        _ = KeepCountdown();
        _timerStatus = TimerStatus.Run;
    }

    private async UniTask KeepCountdown()
    {
        float startTime = Time.time;

        while(_remainingTime > _elapsedTime)
        {
            _elapsedTime = Time.time - startTime;

            await UniTask.Yield(_token.Token); 
        }

        _action.Invoke();
        Stop();
    }

    
}
