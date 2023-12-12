using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializeableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializeableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ConvertToVector3()
    {
        return new Vector3(x, y, z);
    }

    public override string ToString()
    {
        return $"{{{x}, {y}, {z}}}";
    }
}
