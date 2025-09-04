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

    private void Awake()
    {
        _rotationPercentage = RotationIncrement / 360f;
        Debug.Log("Rotation percentage: " + _rotationPercentage);
        _startRotation = transform.eulerAngles.z;
        _endRotation = _startRotation + (direction * 360f * _rotationPercentage);

        Debug.Log($"Start Rotation: {_startRotation} -- End Rotation: {_endRotation}");

        if (WaitAtStart)
        {
            _waiting = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Rotate(Time.deltaTime);
    }

    public void Rotate(float timeSinceLastCall)
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
                    _waiting = true;
                    _timeWaiting = 0f;
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
        else
        {
            if(_timeWaiting >= TimeToWait)
            {
                _waiting = false;
            }
            else
            {
                _timeWaiting = Mathf.Clamp(_timeWaiting + timeSinceLastCall, 0f, TimeToWait);
            }
        }
    }
}
