using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeObjectShifter : MonoBehaviour
{
    /** Settings **/
    [HideInInspector] public Vector3 MovementVector;
    public float TimeToMove = 1f;
    public float TimeToWait = 0f;
    public bool WaitAtStart = false;
    [Range(0f, 2f)] public float StartingPointAlongPath = 0f;
    [SerializeField] private AnimationCurve _timingCurve = AnimationCurve.Linear(0, 0, 1, 1);

    /**Controlling Variables**/
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private float _timeMoving = 0f;
    private float _timeWaiting = 0f;
    private bool _waiting = false;

    private void Awake()
    {
        _startPoint = transform.position;

        _endPoint = _startPoint + MovementVector;

        // Handle different starting positions
        _timeMoving = (StartingPointAlongPath % 1) * TimeToMove;

        if (StartingPointAlongPath >= 1)
        {
            SwapEndpoints();
        }

        // Handle waiting at the beginning
        if (WaitAtStart)
        {
            _waiting = true;
            MovePoint(_timeMoving / TimeToMove);
        }
    }

    private void Update()
    {
        Shift(Time.deltaTime);
    }

    public void Shift(float timeSinceLastCall)
    {

        // If we are not waiting, shift as usual
        if (_waiting == false)
        {
            float movementPercentage = _timeMoving / TimeToMove;
            MovePoint(movementPercentage);

            // If we've reached the end, swap end points, start waiting if need to
            if (_timeMoving >= TimeToMove)
            {
                _timeMoving = 0f;
                SwapEndpoints();

                if (TimeToWait > 0)
                {
                    _waiting = true;
                }
            }
            // Advance time moving
            else
            {
                _timeMoving = Mathf.Clamp(_timeMoving + timeSinceLastCall, 0f, TimeToMove);
            }
        }
        // If we are waiting, advance waiting timer
        else
        {
            // If we're done waiting, stop waiting
            if (_timeWaiting >= TimeToWait)
            {
                _timeWaiting = 0f;
                _waiting = false;
            }
            else
            {
                _timeWaiting = Mathf.Clamp(_timeWaiting + timeSinceLastCall, 0f, TimeToWait);
            }
        }
    }

    /** UTILITIES **/
    public void SwapEndpoints()
    {
        Utilities.SwapVector3(ref _startPoint, ref _endPoint);
    }

    public void MovePoint(float distance)
    {
        float adjustedDistance = _timingCurve.Evaluate(distance);
        transform.position = Vector3.Lerp(_startPoint, _endPoint, adjustedDistance);
    }
}
