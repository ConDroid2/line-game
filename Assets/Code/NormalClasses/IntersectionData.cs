using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionData
{
    public LineController Line { get; private set; }
    public float DistanceAlongLine { get; private set; }

    public IntersectionData(LineController line, float distanceAlongLine)
    {
        Line = line;
        DistanceAlongLine = distanceAlongLine;
    }

    public void SetDistanceAlongLine(float distanceAlongLine)
    {
        DistanceAlongLine = distanceAlongLine;
    }
}
