using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeObjectRotator : MonoBehaviour
{
    [Range(-1, 1)]
    public int direction;
    public float TimeForFullRotation = 1;
    public float TimeToWait = 0f;
    [Range(1, 360)]
    public int RotationIncrement = 360;
    public bool WaitAtStart = false;
    public bool Automatic = true;
    [SerializeField] private AnimationCurve _timingCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float _startRotation;
    private float _endRotation;
    private float _timeMoving = 0f;
    private float _timeWaiting = 0f;
    private bool _waiting = false;
    private float _rotationPercentage;
    private State _currentState = State.Stopped;

    private enum State { Waiting, Rotating, Stopped }

    private void Awake()
    {
        _rotationPercentage = RotationIncrement / 360f;
        Debug.Log("Rotation percentage: " + _rotationPercentage);
        _startRotation = transform.eulerAngles.z;
        _endRotation = _startRotation + (direction * 360f * _rotationPercentage);

        Debug.Log($"Start Rotation: {_startRotation} -- End Rotation: {_endRotation}");

        if (WaitAtStart)
        {
            _currentState = State.Waiting;
        }
        else if(Automatic)
        {
            _currentState = State.Rotating;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case State.Rotating:
                Rotate(Time.deltaTime);
                break;
            case State.Waiting:
                Wait(Time.deltaTime);
                break;
            case State.Stopped:
                break;
            default:
                break;
        }
    }

    private void Rotate(float timeSinceLastCall)
    {
        if (_waiting == false)
        {
            float movementPercentage = _timeMoving / (TimeForFullRotation * _rotationPercentage);

            float adjustedTime = _timingCurve.Evaluate(movementPercentage);
            float newZRotation = Mathf.Lerp(_startRotation, _endRotation, adjustedTime);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);

            if (_timeMoving >= (TimeForFullRotation * _rotationPercentage))
            {
                _timeMoving = 0;
                _startRotation = transform.eulerAngles.z;
                _endRotation = _startRotation + (direction * 360f * _rotationPercentage);

                if (TimeToWait > 0)
                {
                    _currentState = State.Waiting;
                }
                else
                {
                    _currentState = Automatic ? State.Rotating : State.Stopped;
                }
            }
            else
            {
                if (LevelManager.Instance != null)
                {
                    timeSinceLastCall *= LevelManager.Instance.ObjectMovementTimeScale;
                }

                _timeMoving += timeSinceLastCall;
            }
        }
    }

    private void Wait(float timeSinceLastCall)
    {
        if (_timeWaiting >= TimeToWait)
        {
            _currentState = Automatic ? State.Rotating : State.Stopped;
            _timeWaiting = 0f;
        }
        else
        {
            _timeWaiting = Mathf.Clamp(_timeWaiting + timeSinceLastCall, 0f, TimeToWait);
        }
    }

    private void SwitchState(State newState)
    {
        if (_currentState == newState) return;
    }

    public void DoRotation()
    {
        if(_currentState != State.Rotating && Automatic == false)
        {
            _currentState = State.Rotating;
        }
    }

    
}
