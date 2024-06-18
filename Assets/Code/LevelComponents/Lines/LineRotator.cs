using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineRotator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineController _lineController;

    [Header("Settings")]
    [SerializeField] private float _timeForRotation = 1f;
    [SerializeField] private AnimationCurve _rotationCurve;

    // Variables for keeping track
    bool _rotating = false;
    float _timeSinceStarting = 0f;
    float _startRotation;
    float _endRotation;
    Vector3 _startA;
    Vector3 _startB;
    Vector3 _calculatedA;
    Vector3 _calculatedB;

    Quaternion _startQuaternion;
    Quaternion _endQuaternion;

    // Events
    public UnityEvent DoneRotating;

    // Start is called before the first frame update
    void Start()
    {
        _lineController = GetComponent<LineController>();

        // Should all lines be shifting by default?
        _lineController.LineType = Enums.LineType.Shifting;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotating)
        {
            _timeSinceStarting += Time.deltaTime;

            float rotationPercentage = Mathf.Clamp01(_timeSinceStarting / _timeForRotation);
            float adjustedPercentage = _rotationCurve.Evaluate(rotationPercentage);

            //float newZRotation = Mathf.Lerp(_startRotation, _endRotation, adjustedPercentage);

            //transform.eulerAngles = new Vector3(0f, 0f, newZRotation);

            // transform.rotation = Quaternion.Lerp(_startQuaternion, _endQuaternion, adjustedPercentage);

            Vector3 newA = Vector3.Slerp(_startA, _calculatedA, adjustedPercentage);
            Vector3 newB = Vector3.Slerp(_startB, _calculatedB, adjustedPercentage);

            _lineController.SetLocalEndpoint("A", newA);
            _lineController.SetLocalEndpoint("B", newB);

            // _lineController.FixPointOrientation();

            if(_timeSinceStarting >= _timeForRotation)
            {
                _lineController.SetLocalEndpoint("A", _calculatedA);
                _lineController.SetLocalEndpoint("B", _calculatedB);

                _rotating = false;
                DoneRotating.Invoke();

                ValidateNewOrientation();
            }
        }
    }

    public void Rotate()
    {
        if (_rotating) return;

        Vector2 nextSlope = Vector2.Perpendicular(_lineController.CalculateSlope()) * -1;
        float halfLength = _lineController.Length / 2;

        _startA = _lineController.CurrentLocalA;
        _startB = _lineController.CurrentLocalB;

        _calculatedA = -1* new Vector3((nextSlope.normalized * halfLength).x, (nextSlope.normalized * halfLength).y);
        _calculatedB = new Vector3((nextSlope.normalized * halfLength).x, (nextSlope.normalized * halfLength).y);

        //Debug.Log($"Next slope is: {nextSlope.x}, {nextSlope.y}");
        //Debug.Log($"Half length is: {halfLength}");
        //Debug.Log($"New A.x is: {_calculatedA.x}");
        //Debug.Log($"New A.y is: {_calculatedA.y}");

        _timeSinceStarting = 0;

        _lineController.LineType = Enums.LineType.Shifting;
        // stop player input
        _rotating = true;
    }

    public void Reverse()
    {
        if (_rotating) return;
        Debug.Log("Start Reversing");
        

        Vector2 nextSlope = Vector2.Perpendicular(_lineController.CalculateSlope());
        float halfLength = _lineController.Length / 2;

        _startA = _lineController.CurrentLocalA;
        _startB = _lineController.CurrentLocalB;

        _calculatedA = -1 * new Vector3((nextSlope.normalized * halfLength).x, (nextSlope.normalized * halfLength).y);
        _calculatedB = new Vector3((nextSlope.normalized * halfLength).x, (nextSlope.normalized * halfLength).y);

        //Debug.Log($"Next slope is: {nextSlope.x}, {nextSlope.y}");
        //Debug.Log($"Half length is: {halfLength}");
        //Debug.Log($"New A.x is: {_calculatedA.x}");
        //Debug.Log($"New A.y is: {_calculatedA.y}");

        _timeSinceStarting = 0;

        _lineController.LineType = Enums.LineType.Shifting;
        _rotating = true;
    }

    public void ValidateNewOrientation()
    {
        Debug.Log("Validating new orientation");
        //_lineController.SetEndpoint("A", FixEndpointPositions(_lineController.CurrentA));
        //_lineController.SetEndpoint("B", FixEndpointPositions(_lineController.CurrentB));
        _lineController.FixPointOrientation();

        Vector3 a = _lineController.CurrentA;
        Vector3 b = _lineController.CurrentB;

        // Check if either point is outside level bounds
        if(CheckIfPointIsOutsideLevel(a) || CheckIfPointIsOutsideLevel(b))
        {
            Reverse();
            return;
        }

        // Get level manager
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        foreach(LineController line in levelManager.Lines)
        {
            if (line == _lineController || line.Active == false) continue;
            // Debug.Log("Checking line");
            Utilities.IntersectionPoint intersectionPoint = Utilities.FindIntersectionPoint(a, b, line.CurrentA, line.CurrentB);
            // Debug.Log($"{intersectionPoint.Point}, {intersectionPoint.IsParallel}");

            if (intersectionPoint.IsParallel && intersectionPoint.Point.x == Vector3.negativeInfinity.x)
            {
                Debug.Log("Potential Problem");
                Reverse();
            }
        }
    }

    private Vector3 FixEndpointPositions(Vector3 endpoint)
    {
        decimal x = System.Math.Round((decimal)endpoint.x, 2);
        decimal y = System.Math.Round((decimal)endpoint.y, 2);
        decimal z = System.Math.Round((decimal)endpoint.z, 2);

        // Debug.Log($"Initial x: {endpoint.y}, Fixed x: {(float)y}");

        return new Vector3((float)x, (float)y, (float)z);
    }

    private bool CheckIfPointIsOutsideLevel(Vector3 point)
    {
        return point.x < LevelManager.Instance.RoomLeftSide || point.x > LevelManager.Instance.RoomRightSide || point.y < LevelManager.Instance.RoomBottomSide || point.y > LevelManager.Instance.RoomTopSide;
    }
}
