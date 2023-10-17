using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 Round( this Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);

        return v;
    }

    public static Vector3 Round(this Vector3 v, float size)
    {
        return (v / size).Round() * size;
    }

    public static Vector2 AbsoluteValue(this Vector2 v)
    {
        v.x = Mathf.Abs(v.x);
        v.y = Mathf.Abs(v.y);

        return v;
    }
}
