using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle
{
    private Vector2 _pointA;
    private Vector2 _pointB;
    private Vector2 _pointC;
    private Vector2 _pointD;

    // Bounds if not Slanted
    private float _xMax;
    private float _xMin;
    private float _yMax;
    private float _yMin;

    public Vector2 PointA => _pointA;
    public Vector2 PointB => _pointB;
    public Vector2 PointC => _pointC;
    public Vector2 PointD => _pointD;

    public bool Slanted { get; private set; }

    // points need to be added in order (a needs to connect to b needs to connect to c ...)
    public Rectangle(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        _pointA = a;
        _pointB = b;
        _pointC = c;
        _pointD = d;

        Slanted = CalculateIfSlanted();

        if (!Slanted) CalculateBounds();
    }

    public void CalculateBounds()
    {
        Vector2[] points = { _pointA, _pointB, _pointC, _pointD };

        _xMax = 0f;
        _xMin = float.MaxValue;
        _yMax = 0f;
        _yMin = float.MaxValue;

        foreach (Vector2 point in points)
        {
            if (point.x > _xMax) _xMax = point.x;
            if (point.x < _xMin) _xMin = point.x;
            if (point.y > _yMax) _yMax = point.y;
            if (point.y < _yMin) _yMin = point.y;
        }
    }

    public bool PointInRectangle(Vector2 position)
    {
        bool isInRectangle = false;
        if (!Slanted)
        {
            isInRectangle = position.x < _xMax && position.x > _xMin && position.y < _yMax && position.y > _yMin;
        }
        else
        {
            // Do other thing
        }

        return isInRectangle;
    }

    private bool CalculateIfSlanted()
    {
        // If any slope between a and another point is flat, rectangle is not slanted
        bool notSlanted = false;

        notSlanted |= _pointB.x - _pointA.x == 0;
        notSlanted |= _pointC.x - _pointA.x == 0;
        notSlanted |= _pointD.x - _pointA.x == 0;

        Debug.Log("Am I slanted? " + !notSlanted);

        return !notSlanted;
    }

    private bool PointInTriangle(Vector2 pointA, Vector2 pointB, Vector2 pointC, Vector2 position)
    {
        Vector2 v0 = pointC - pointA;
        Vector2 v1 = pointB - pointA;
        Vector2 v2 = position - pointA;
        return false;
    }
}


