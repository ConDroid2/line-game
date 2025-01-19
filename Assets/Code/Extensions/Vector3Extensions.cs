using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static void Swap(this Vector3 v, ref Vector3 w)
    {
        Vector3 temp = Vector3.zero;

        temp.Set(v.x, v.y, v.z);
        v.Set(w.x, w.y, w.z);
        w.Set(temp.x, temp.y, temp.z);
    }
}
