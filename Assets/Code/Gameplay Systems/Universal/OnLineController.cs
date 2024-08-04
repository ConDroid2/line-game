using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLineController : MonoBehaviour
{
    public LineController CurrentLine;

    public Enums.ObjectType ObjectType;

    [SerializeField] private bool _fixDistanceOnAwake;

    private float _distanceOnLine;
    public float DistanceOnLine
    {
        get { return _distanceOnLine; }
        set { SetDistanceOnLine(value); }
    }

    private bool _useCurrentLineData = false;

    private void Awake()
    {
        _useCurrentLineData = true;
    }

    private void Start()
    {
        if (_fixDistanceOnAwake)
        {
            Vector3 ab = CurrentLine.CurrentB - CurrentLine.CurrentA;
            Vector3 av = transform.position - CurrentLine.CurrentA;

            DistanceOnLine = Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
            if(CurrentLine.OnLineControllers.Contains(this) == false) CurrentLine.AddOnLine(this);
        }
    }

    private void Update()
    {
        if(CurrentLine.LineType == Enums.LineType.Shifting)
        {
            DistanceOnLine = DistanceOnLine;
        }
    }

    private void SetDistanceOnLine(float newDistance)
    {
        _distanceOnLine = Mathf.Clamp(newDistance, 0f, 1f);
        // Debug.Log($"Attempted Distance: {newDistance} vs Clamped Distance: {_distanceOnLine}");
        Vector3 A = _useCurrentLineData ? CurrentLine.CurrentA : CurrentLine.InitialA;
        Vector3 B = _useCurrentLineData ? CurrentLine.CurrentB : CurrentLine.InitialB;

        transform.position = Vector3.Lerp(A, B, DistanceOnLine);
        // Not sure how I feel about this, but it works for now
        transform.right = _useCurrentLineData ? CurrentLine.CalculateSlope() : CurrentLine.CalculateInitialSlope();
    }

    public Vector3 CheckNewPosition(float newDistance)
    {
        newDistance = Mathf.Clamp(newDistance, 0f, 1f);
        return Vector3.Lerp(CurrentLine.CurrentA, CurrentLine.CurrentB, newDistance);
    }

    public void SetLine(LineController newLine, float distanceOnLine = 0)
    {
        if (CurrentLine != null)
        {
            CurrentLine.RemoveFromLine(this);
        }

        CurrentLine = newLine;

        if (CurrentLine != null)
        {
            CurrentLine.AddOnLine(this);
            SetDistanceOnLine(distanceOnLine);
        }
    }

    public void SetLineInEvent(LineController newLine)
    {
        SetLine(newLine);
    }

#if UNITY_EDITOR
    public void SetDistanceOnLineInEditor(float newDistance)
    {
        _distanceOnLine = Mathf.Clamp(newDistance, 0f, 1f);
        transform.position = Vector3.Lerp(CurrentLine.InitialA, CurrentLine.InitialB, DistanceOnLine);
        // Not sure how I feel about this, but it works for now
        transform.right = CurrentLine.CalculateSlope();

        transform.position = transform.position.Round(0.5f);
    }
#endif
}
