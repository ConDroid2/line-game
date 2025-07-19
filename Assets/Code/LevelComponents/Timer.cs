using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private bool _startOnRoomStart = false;

    // Variables to keep track
    private float _timeSinceStart;
    private bool _running = false;

    private bool _paused = false;

    // Events
    public UnityEvent OnTimerEnd;
    public UnityEvent<float> OnPercentDoneChange;

    // properties
    public float PercentDone => _timeSinceStart / _time;

    private void Start()
    {
        if (_startOnRoomStart)
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (_running && _paused == false)
        {
            _timeSinceStart += Time.deltaTime;

            OnPercentDoneChange.Invoke(PercentDone);

            // If timer done, send event
            if(_timeSinceStart >= _time)
            {  
                _running = false;
                OnTimerEnd?.Invoke();
            }
        }
    }

    public void StartTimer()
    {
        // Start timer if not running already
        if (_running == false)
        {
            _timeSinceStart = 0f;
            _running = true;
        }
    }

    public void PauseTimer()
    {
        _paused = true;
    }

    public void UnPauseTimer()
    {
        _paused = false;
    }

    public void StopTimerWithoutSendingEvent()
    {
        _running = false;
    }

    public void StopTimerAndSendEvent()
    {
        _running = false;
        OnTimerEnd.Invoke();
    }
}
