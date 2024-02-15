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

            float newZRotation = Mathf.Lerp(_startRotation, _endRotation, adjustedPercentage);

            transform.eulerAngles = new Vector3(0f, 0f, newZRotation);

            _lineController.FixPointOrientation();

            if(_timeSinceStarting >= _timeForRotation)
            {
                _rotating = false;
                DoneRotating.Invoke();
                // Allow player to move'
                // Need to do some kind of check to see if rotation is valid
                ValidateNewOrientation();
            }
        }
    }

    public void Rotate()
    {
        if (_rotating) return;
        // Debug.Log("Start Rotating");
        _startRotation = transform.eulerAngles.z;
        _endRotation = _startRotation - 90f;
        _timeSinceStarting = 0;

        _lineController.LineType = Enums.LineType.Shifting;
        // stop player input
        _rotating = true;
    }

    public void Reverse()
    {
        if (_rotating) return;
        Debug.Log("Start Reversing");
        _startRotation = transform.eulerAngles.z;
        _endRotation = _startRotation + 90f;
        _timeSinceStarting = 0;

        _lineController.LineType = Enums.LineType.Shifting;
        _rotating = true;
    }

    public void ValidateNewOrientation()
    {
        Vector3 a = _lineController.CurrentA;
        Vector3 b = _lineController.CurrentB;

        // Get level manager
        LevelManager levelManager = FindObjectOfType<LevelManager>();

        foreach(LineController line in levelManager.Lines)
        {
            if (line == _lineController) continue;
            // Debug.Log("Checking line");
            Utilities.IntersectionPoint intersectionPoint = Utilities.FindIntersectionPoint(a, b, line.CurrentA, line.CurrentB);
            // Debug.Log($"{intersectionPoint.Point}, {intersectionPoint.IsParallel}");

            if (intersectionPoint.IsParallel && intersectionPoint.Point.x == Vector3.negativeInfinity.x)
            {
                // Debug.Log("Potential Problem");
                Reverse();
            }
        }
        // Check if it has any intersections that are Positive Infinity and Parallel
        // Check if this line is inside any of those
        // Check if either endpoint is outside level bounds
        // If not valid, rotate back

        //Debug.Log($"{_lineController.CurrentA.y}");
        //Debug.Log($"{System.Math.Round((decimal)_lineController.CurrentA.y, 2)}");
    }
}
