using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineShifter
{
    /**Settings**/
    [HideInInspector] public Vector3 MovementVector;
    public float TimeToMove = 1f;
    public float TimeToWait = 0f;
    public bool WaitAtStart = false;
    [Range(0f, 2f)] public float StartingPointAlongPath = 0f;

    /**Controlling Variables**/
    private Vector3 _aStartPoint;
    private Vector3 _bStartPoint;
    private Vector3 _aEndPoint;
    private Vector3 _bEndPoint;
    private float _timeMoving = 0f;
    private float _timeWaiting = 0f;
    private bool _waiting = false;

    private LineController _lineController;

    public void SetUp(LineController lineController)
    {
        _lineController = lineController;

        _aStartPoint = _lineController.A;
        _bStartPoint = _lineController.B;

        _aEndPoint = _aStartPoint + MovementVector;
        _bEndPoint = _bStartPoint + MovementVector;

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
            MovePoints(_timeMoving / TimeToMove);
        }
    }

    public void Shift(float timeSinceLastCall)
    {   

        // If we are not waiting, shift as usual
        if (_waiting == false)
        {
            float movementPercentage = _timeMoving / TimeToMove;
            MovePoints(movementPercentage);

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
            if(_timeWaiting >= TimeToWait)
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

    public void SwapEndpoints()
    {
        Utilities.SwapVector3(ref _aStartPoint, ref _aEndPoint);
        Utilities.SwapVector3(ref _bStartPoint, ref _bEndPoint);
    }

    public void MovePoints(float distance)
    {
        _lineController.A = Vector3.Lerp(_aStartPoint, _aEndPoint, distance);
        _lineController.B = Vector3.Lerp(_bStartPoint, _bEndPoint, distance);
    }
}
