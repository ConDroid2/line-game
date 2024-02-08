using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Force
{
    public float Magnitude { get; private set; }
    public Vector2 Direction { get; private set; }
    public float Drag { get; private set; }
    public GameObject Source { get; private set; }

    public Force(float magnitude, Vector2 direction, float drag, GameObject source)
    {
        Magnitude = magnitude;
        Direction = direction.normalized;
        Drag = drag;
        Source = source;
    }

    public void ApplyDrag()
    {
        Magnitude = Magnitude * Drag;
    }

    public override string ToString()
    {
        return $"Magnitude: {Magnitude}, Direction {Direction}, Drag {Drag}, Source {Source.name}";
    }
}
