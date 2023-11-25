using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeObjectRotator : MonoBehaviour
{
    public float TimeForFullRotation = 1;
    public float TimeToWait = 0f;
    public bool WaitAtStart = false;
    [SerializeField] private AnimationCurve _timingCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float _startRotation;
    private float _endRotation;
    private float _timeMoving = 0f;
    private float _timeWaiting = 0f;
    private bool _waiting = false;

    private void Awake()
    {
        _startRotation = transform.eulerAngles.z;
        _endRotation = 360f;

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
            float movementPercentage = _timeMoving / TimeForFullRotation;

            float adjustedTime = _timingCurve.Evaluate(movementPercentage);
            float newZRotation = Mathf.Lerp(_startRotation, _endRotation, adjustedTime);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, newZRotation);

            if (_timeMoving >= TimeForFullRotation)
            {
                _timeMoving = 0;

                if(TimeToWait > 0)
                {
                    _waiting = true;
                    _timeWaiting = 0f;
                }
            }
            else
            {
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
