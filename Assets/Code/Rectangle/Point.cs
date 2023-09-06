using System.Collections;
using System.Collections.Generic;

public class Point
{
    private float _x;
    private float _y;

    public float X => _x;
    public float Y => _y;

    public Point(float x, float y)
    {
        _x = x;
        _y = y;
    }
}
