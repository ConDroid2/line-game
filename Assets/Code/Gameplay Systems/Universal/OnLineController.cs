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

    private void Start()
    {
        if (_fixDistanceOnAwake)
        {
            Vector3 ab = CurrentLine.CurrentB - CurrentLine.CurrentA;
            Vector3 av = transform.position - CurrentLine.CurrentA;

            DistanceOnLine = Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
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
        transform.position = Vector3.Lerp(CurrentLine.CurrentA, CurrentLine.CurrentB, DistanceOnLine);
        // Not sure how I feel about this, but it works for now
        transform.right = CurrentLine.CalculateSlope();
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
        CurrentLine.AddOnLine(this);
        SetDistanceOnLine(distanceOnLine);
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
