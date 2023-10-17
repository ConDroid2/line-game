using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnLine : MonoBehaviour
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

    private void Awake()
    {
        if (_fixDistanceOnAwake)
        {
            Vector3 ab = CurrentLine.B - CurrentLine.A;
            Vector3 av = transform.position - CurrentLine.A;

            DistanceOnLine = Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
        }
    }

    private void SetDistanceOnLine(float newDistance)
    {
        _distanceOnLine = Mathf.Clamp(newDistance, 0f, 1f);
        transform.position = Vector3.Lerp(CurrentLine.A, CurrentLine.B, DistanceOnLine);
    }

    public Vector3 CheckNewPosition(float newDistance)
    {
        newDistance = Mathf.Clamp(newDistance, 0f, 1f);
        return Vector3.Lerp(CurrentLine.A, CurrentLine.B, newDistance);
    }

    public void SetLine(LineController newLine, float distanceOnLine = 0)
    {
        CurrentLine = newLine;
        SetDistanceOnLine(distanceOnLine);
    }
}
