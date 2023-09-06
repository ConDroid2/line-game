using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LineShifter
{
    /**Settings**/
    public Vector3 MovementVector;
    public float TimeToMove;

    /**Controlling Variables**/
    private Vector3 _aStartPoint;
    private Vector3 _bStartPoint;
    private Vector3 _aEndPoint;
    private Vector3 _bEndPoint;
    private float _timeMoving = 0f;

    private LineController _lineController;

    public void SetUp(LineController lineController)
    {
        _lineController = lineController;

        _aStartPoint = _lineController.A;
        _bStartPoint = _lineController.B;

        _aEndPoint = _aStartPoint + MovementVector;
        _bEndPoint = _bStartPoint + MovementVector;
    }

    public void Move(float timeSinceLastCall)
    {
        float movementPercentage = _timeMoving / TimeToMove;

        _lineController.A = Vector3.Lerp(_aStartPoint, _aEndPoint, movementPercentage);
        _lineController.B = Vector3.Lerp(_bStartPoint, _bEndPoint, movementPercentage);

        if (_timeMoving >= TimeToMove)
        {
            _timeMoving = 0f;
            Utilities.SwapVector3(ref _aStartPoint, ref _aEndPoint);
            Utilities.SwapVector3(ref _bStartPoint, ref _bEndPoint);
        }

        _timeMoving = Mathf.Clamp(_timeMoving + timeSinceLastCall, 0f, TimeToMove);
    }
}
