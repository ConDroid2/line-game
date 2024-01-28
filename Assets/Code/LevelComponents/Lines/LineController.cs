using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    // Leaving these here just for now in case the editor bug wasn't fixed
    //public SerializeableVector3 TestA = new SerializeableVector3(1, 1, 0);
    //public SerializeableVector3 TestB = new SerializeableVector3(1, 0, 0);
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

    private List<OnLineController> _onLineControllers = new List<OnLineController>();

    //[Header("Moving line stuff")]
    //public LineShifter[] LineShifters = new LineShifter[1];
    private bool _needsSlopeUpdates = false;

    private void Awake()
    {
        _transformA.position = InitialA;
        _transformB.position = InitialB;
    }

    private void Start()
    {
        bool hasFreeObjectShifter = GetComponent<FreeObjectShifter>() != null;
        bool hasFreeObjectRotator = GetComponent<FreeObjectRotator>() != null;
        if (hasFreeObjectRotator || hasFreeObjectShifter)
        {
            LineType = Enums.LineType.Shifting;
        }

        if (hasFreeObjectRotator)
        {
            _needsSlopeUpdates = true;
        }
    }

    private void Update()
    {
        if (_needsSlopeUpdates)
        {
            FixPointOrientation();
        }
        Slope = CalculateSlope();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FixPointOrientation();
        }
    }


    public void ConfigureInformation()
    {
        FixInitialPointOrientation();
        FixPointOrientation();
        Slope = CalculateSlope();
        SlopeType = Utilities.DetermineSlopeType(Slope.x, Slope.y);

        // Caluclate Direction Modifier
        CalculateDirectionModifier();
    }

    public Vector2 CalculateSlope()
    {
        float slopeX = CurrentB.x - CurrentA.x;
        float slopeY = CurrentB.y - CurrentA.y;

        //Debug.Log(slopeX);
        //Debug.Log(slopeY);

        return new Vector2(slopeX, slopeY).normalized;
    }

    public Vector2 CalculateInitialSlope()
    {
        float slopeX = InitialB.x - InitialA.x;
        float slopeY = InitialB.y - InitialA.y;

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

    // Make it so moving from A -> B is always "positive"
    public void FixPointOrientation()
    {
        if(CurrentB.x < CurrentA.x || (CurrentB.x - CurrentA.x == 0 && CurrentB.y < CurrentA.y))
        {
            Vector3 temp = _transformB.position;
            _transformB.position = _transformA.position;
            _transformA.position = temp;

            foreach (OnLineController onLine in _onLineControllers)
            {
                float newDistanceOnLine = 1 - onLine.DistanceOnLine;
                onLine.DistanceOnLine = newDistanceOnLine;
            }
        }
    }

    public void FixInitialPointOrientation()
    {
        if(InitialB.x < InitialA.x || (InitialB.x - InitialA.x == 0 && InitialB.y < InitialA.y))
        {
            Vector3 temp = InitialB;
            InitialB = InitialA;
            InitialA = temp;

            foreach(OnLineController onLine in _onLineControllers)
            {
                float newDistanceOnLine = 1 - onLine.DistanceOnLine;
                onLine.DistanceOnLine = newDistanceOnLine;
            }
        }
    }

    public void AddOnLine(OnLineController newObject)
    {
        if(_onLineControllers.Contains(newObject) == false)
        {
            _onLineControllers.Add(newObject);
        }
    }

    public void RemoveFromLine(OnLineController objectOnLine)
    {
        if (_onLineControllers.Contains(objectOnLine))
        {
            _onLineControllers.Remove(objectOnLine);
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
