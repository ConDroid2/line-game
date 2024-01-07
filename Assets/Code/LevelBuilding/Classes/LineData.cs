using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineData
{
    public SerializeableVector3 A;
    public SerializeableVector3 B;
    public int SiblingIndex;

    public LineData(Vector3 newA, Vector3 newB, int siblingIndex)
    {
        A = new SerializeableVector3(newA);
        B = new SerializeableVector3(newB);
        SiblingIndex = siblingIndex;
    }

    
}
