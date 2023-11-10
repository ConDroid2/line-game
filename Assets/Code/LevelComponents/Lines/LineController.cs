using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public Vector3 InitialA = new Vector3(1, 1, 0);
    public Vector3 InitialB = new Vector3(1, 0, 0);
    public Vector3 InitialMidpoint => new Vector3((InitialA.x + InitialB.x) / 2, (InitialA.y + InitialB.y) / 2);
    
    [SerializeField] private Transform _transformA;
    [SerializeField] private Transform _transformB;
    public Vector3 CurrentA => _transformA.position;
    public Vector3 CurrentB => _transformB.position;
    public Vector3 CurrentMidpoint => new Vector3((CurrentA.x + CurrentB.x) / 2, (CurrentA.y + CurrentB.y) / 2);

    public Enums.SlopeType SlopeType { get; private set; }
    public Enums.LineType LineType = Enums.LineType.Static;

    public float DirectionModifier = 1;

    public Vector2 Slope = new Vector2();
    public float Length => CalculateLength();

    //[Header("Moving line stuff")]
    //public LineShifter[] LineShifters = new LineShifter[1];

    private void Awake()
    {
        _transformA.position = InitialA;
        _transformB.position = InitialB;
    }

    private void Start()
    {
        if(GetComponent<FreeObjectShifter>() != null)
        {
            LineType = Enums.LineType.Shifting;
        }
    }


    public void ConfigureInformation()
    {
        Slope = CalculateSlope();
        SlopeType = Utilities.DetermineSlopeType(Slope.x, Slope.y);

        //if(LineShifters.Length > 0)
        //{
        //    Debug.Log("Setting up line shifter");
        //    LineType = Enums.LineType.Shifting;

        //    foreach (LineShifter lineShifter in LineShifters)
        //    {
        //        lineShifter.SetUp(this);
        //    }
        //}

        // Caluclate Direction Modifier
        CalculateDirectionModifier();
    }

    public Vector2 CalculateSlope()
    {
        float slopeX = CurrentB.x - CurrentA.x;
        float slopeY = CurrentB.y - CurrentA.y;

        return new Vector2(slopeX, slopeY).normalized;
    }

    private float CalculateLength()
    {
        return Vector3.Distance(InitialA, InitialB);
    }

    public Vector3 CalculateMidpoint()
    {
        float x = (InitialA.x + InitialB.x) / 2;
        float y = (InitialA.y + InitialB.y) / 2;
        float z = (InitialA.z + InitialB.z) / 2;

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
            Gizmos.DrawLine(InitialA, InitialB);
        }
    }



    //private void OnDrawGizmosSelected()
    //{
    //    if (LineShifters.Length > 0)
    //    {
    //        Gizmos.color = Color.green;

    //        foreach (LineShifter lineShifter in LineShifters)
    //        {
    //            Vector3 newA = InitialA + lineShifter.MovementVector;
    //            Vector3 newB = InitialB + lineShifter.MovementVector;

    //            Gizmos.DrawLine(newA, newB);
    //            Gizmos.DrawLine(Midpoint, Midpoint + lineShifter.MovementVector);
    //        }    
    //    }

    //}
}
