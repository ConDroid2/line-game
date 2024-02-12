using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _time;

    // Variables to keep track
    private float _timeSinceStart;
    private bool _running = false;

    // Events
    public UnityEvent OnTimerEnd;

    // properties
    public float PercentDone => _timeSinceStart / _time;

    void Update()
    {
        if (_running)
        {
            _timeSinceStart += Time.deltaTime;

            // If timer done, send event
            if(_timeSinceStart >= _time)
            {
                OnTimerEnd.Invoke();
                _running = false;
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
}
