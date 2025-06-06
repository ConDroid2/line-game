using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LineRotator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineController _lineController;

    [Header("Settings")]
    [SerializeField] private float _timeForFullRotation = 1f;
    [SerializeField] private AnimationCurve _rotationCurve;
    [SerializeField] private ContactFilter2D _obstructionFilter = new ContactFilter2D();

    // Variables for keeping track
    bool _rotating = false;
    float _timeSinceStarting = 0f;
    float _timeForRotation = 0f;
    bool _checkCollisions = true;
    float _startRotation;
    float _endRotation;
    Vector3 _startA;
    Vector3 _startB;
    Vector3 _calculatedA;
    Vector3 _calculatedB;

    Quaternion _startQuaternion;
    Quaternion _endQuaternion;

    // Events
    public UnityEvent OnStartRotating;
    public UnityEvent DoneRotating;
    public UnityEvent OnInvalidRotation;

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
            float timeSinceLastUpdate = Time.deltaTime;

            if (LevelManager.Instance != null)
            {
                timeSinceLastUpdate *= LevelManager.Instance.ObjectMovementTimeScale;
            }

            _timeSinceStarting += timeSinceLastUpdate;

            float rotationPercentage = Mathf.Clamp01(_timeSinceStarting / _timeForRotation);

           

            float adjustedPercentage = _rotationCurve.Evaluate(rotationPercentage);

            Vector3 newA = Vector3.Slerp(_startA, _calculatedA, adjustedPercentage);
            Vector3 newB = Vector3.Slerp(_startB, _calculatedB, adjustedPercentage);

            _lineController.SetLocalEndpoint("A", newA);
            _lineController.SetLocalEndpoint("B", newB);

            // _lineController.FixPointOrientation();

            if (_timeSinceStarting >= _timeForRotation)
            {
                _lineController.SetLocalEndpoint("A", _calculatedA);
                _lineController.SetLocalEndpoint("B", _calculatedB);

                _rotating = false;
                _checkCollisions = true;
                DoneRotating.Invoke();

                ValidateNewOrientation();
            }
            else if (_checkCollisions && CheckCollisions())
            {
                Debug.Log("Found collision, can't rotate");
                _rotating = false;
                _checkCollisions = false;
                OnInvalidRotation?.Invoke();
                Reverse();
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

        _timeForRotation = _timeForFullRotation;
        _timeSinceStarting = 0;

        _lineController.LineType = Enums.LineType.Shifting;
        // stop player input
        _rotating = true;
        OnStartRotating?.Invoke();
    }

    public void Reverse()
    {
        if (_rotating) return;
        // _lineController.FixPointOrientation();

        Vector3 temp = Vector3.zero;

        temp.Set(_startA.x, _startA.y, _startA.z);
        _startA = _lineController.CurrentLocalA;
        _calculatedA.Set(temp.x, temp.y, temp.z);

        temp.Set(_startB.x, _startB.y, _startB.z);
        _startB = _lineController.CurrentLocalB;
        _calculatedB.Set(temp.x, temp.y, temp.z);

        _timeSinceStarting = 0;
        _timeForRotation = _timeForFullRotation;

        _lineController.LineType = Enums.LineType.Shifting;
        _rotating = true;
        OnStartRotating?.Invoke();
    }

    public void ValidateNewOrientation()
    {
        _lineController.FixPointOrientation();

        Vector3 a = _lineController.CurrentA;
        Vector3 b = _lineController.CurrentB;

        // Check if either point is outside level bounds
        if(CheckIfPointIsOutsideLevel(a) || CheckIfPointIsOutsideLevel(b))
        {
            OnInvalidRotation?.Invoke();
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
                OnInvalidRotation?.Invoke();
                Reverse();
                return;
            }
        }
    }

    private bool CheckCollisions()
    {
        int hits = 0;

        foreach(OnLineController onLine in _lineController.OnLineControllers)
        {
            var movementController = onLine.GetComponent<LineMovementController>();
            if (movementController == null) continue;

            Collider2D[] results = new Collider2D[10];

            int numberOfHits = Physics2D.OverlapBox(onLine.transform.position, movementController.Collider.bounds.size, transform.eulerAngles.z, _obstructionFilter, results);

            for(int i = 0; i < numberOfHits; i++)
            {   
                if (results[i] != null &&
                    results[i].gameObject != onLine.gameObject && // Ignore self
                    results[i].GetComponent<OnLineController>().CurrentLine != _lineController) // Ignore objects on this same line
                {
                    hits++; 
                }
            }
        }

        return hits > 0;
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
