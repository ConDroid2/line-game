using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Vector3 A = new Vector3(1, 1, 0);
    public Vector3 B = new Vector3(1, 0, 0);
    public Vector3 Midpoint => new Vector3((A.x + B.x) / 2,(A.y + B.y) / 2);

    public Enums.SlopeType SlopeType { get; private set; }
    public Enums.LineType LineType = Enums.LineType.Static;

    public float DirectionModifier = 1;


    public Rectangle Area { get; private set; }

    public Vector2 Slope = new Vector2();
    public float Length => CalculateLength();

    [Header("Moving line stuff")]
    public LineShifter[] LineShifters = new LineShifter[1];


    private void Update()
    {
        if(LineShifters.Length > 0)
        {
            foreach (LineShifter lineShifter in LineShifters)
            {
                lineShifter.Shift(Time.deltaTime);
            }
        }
    }

    public void ConfigureInformation()
    {
        Slope = CalculateSlope();
        SlopeType = Utilities.DetermineSlopeType(Slope.x, Slope.y);

        if(LineShifters.Length > 0)
        {
            Debug.Log("Setting up line shifter");
            LineType = Enums.LineType.Shifting;

            foreach (LineShifter lineShifter in LineShifters)
            {
                lineShifter.SetUp(this);
            }
        }

        // Caluclate Direction Modifier
        CalculateDirectionModifier();
    }

    private Vector2 CalculateSlope()
    {
        float slopeX = B.x - A.x;
        float slopeY = B.y - A.y;

        return new Vector2(slopeX, slopeY).normalized;
    }

    private float CalculateLength()
    {
        return Vector3.Distance(A, B);
    }

    public Vector3 CalculateMidpoint()
    {
        float x = (A.x + B.x) / 2;
        float y = (A.y + B.y) / 2;
        float z = (A.z + B.z) / 2;

        return new Vector3(x, y, z);
    }

    private void CalculateDirectionModifier()
    {
        if (SlopeType == Enums.SlopeType.Horizontal)
        {
            DirectionModifier = Slope.x;
        }
        else if (SlopeType == Enums.SlopeType.Vertical)
        {
            DirectionModifier = Slope.y;
        }
        else if (SlopeType == Enums.SlopeType.Ascending || SlopeType == Enums.SlopeType.Descending)
        {
            DirectionModifier = Slope.x > 0 ? 1 : -1; // Doesn't matter if we use x or y here since they should be the same
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
        {
            Gizmos.DrawLine(A, B);
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (LineShifters.Length > 0)
        {
            Gizmos.color = Color.green;

            foreach (LineShifter lineShifter in LineShifters)
            {
                Vector3 newA = A + lineShifter.MovementVector;
                Vector3 newB = B + lineShifter.MovementVector;

                Gizmos.DrawLine(newA, newB);
                Gizmos.DrawLine(Midpoint, Midpoint + lineShifter.MovementVector);
            }    
        }

    }



}
