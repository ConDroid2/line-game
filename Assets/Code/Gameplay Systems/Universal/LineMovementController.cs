using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovementController : MonoBehaviour
{
    // External References
    // public LineController _currentLine;
    public OnLineController OnLineController;
    public LevelManager LevelManager { get; private set; }

    public float LineDirectionModifier { get; private set; } // Makes it so we can move correctly regardless of slope sign
    public float CheckForIntersectionsDistance; // The distance we check for intersections
    [SerializeField] private bool _canPush;
    [SerializeField] private float _pushableDistanceCheck;

    public System.Action<Vector3> OnReachedEdgeOfLine;
    public System.Action<int> OnTryToMoveInDirection;

    // privates
    private float _angleBetweenInputAndSlope;
    private Collider2D _collider;

    private void Awake()
    {
        if (_canPush)
        {
            _collider = GetComponent<Collider2D>();
        }
    }

    /*
     * @author Connor Davis
     * @description Attempt to move along the line. Return false if didn't move, true if did
     */
    public bool MoveAlongLine(Vector2 inputVector, float distanceToMove)
    {
        LineController currentLine = OnLineController.CurrentLine;

        // If we get sent a zero vector for trying to move, set position as same
        if (inputVector == Vector2.zero)
        {
            OnLineController.DistanceOnLine = OnLineController.DistanceOnLine;

            return false;
        }

        HandleSwappingLines(inputVector, currentLine);

        float newDistanceOnLine = OnLineController.DistanceOnLine;
        float positionDelta = distanceToMove * currentLine.DirectionModifier;
        _angleBetweenInputAndSlope = Vector3.Angle(inputVector.normalized, currentLine.Slope * currentLine.DirectionModifier);


        // Actually move the object along the line
        // If holding in the positive direction
        if (_angleBetweenInputAndSlope < 90f)
        {
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine + positionDelta, 0f, 1f);
            OnTryToMoveInDirection.Invoke(1);
        }
        // If holding in the negative direction
        else if (_angleBetweenInputAndSlope > 90f)
        {
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine - positionDelta, 0f, 1f);
            OnTryToMoveInDirection.Invoke(-1);
        }

        // Check what our new position will be
        Vector3 attemptedNewPosition = OnLineController.CheckNewPosition(newDistanceOnLine);

        // Keep of track of if we are actually moving
        bool willMove = attemptedNewPosition != transform.position;

        if (_canPush)
        {
            // Handle colslisions if need to
            // Should be the movement vector, not the input vector because they could be different
            Vector3 movementVector = (attemptedNewPosition - transform.position).normalized;
            willMove &= HandleCollisions(movementVector, distanceToMove);
        }

        // If we are moving/can move, do it and return that
        if (willMove)
        {
            OnLineController.DistanceOnLine = newDistanceOnLine;

            if(OnLineController.DistanceOnLine == 0 || OnLineController.DistanceOnLine == 1)
            {
                OnReachedEdgeOfLine?.Invoke(transform.position);
                
            }
        }

        return willMove;
    }


    /**
     * @author Connor Davis
     * @description Set a new line
     */
    public void SetNewLine(LineController newLine, float distanceOnLine = 0)
    {
        OnLineController.SetLine(newLine, distanceOnLine);
    }

    /**
     * @author Connor Davis
     * @description Check if we should be moving to a new line based on input, then move to it
     */
    private void HandleSwappingLines(Vector2 inputVector, LineController currentLine)
    {
        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(currentLine, transform.position, CheckForIntersectionsDistance);

        foreach (IntersectionData intersection in intersections)
        {
            // Check if the input would allow movment along the intersecting line
            float toleranceAngle = 20f;

            
            bool canMoveToNewLine = Vector3.Angle(inputVector.normalized, intersection.Line.Slope) < toleranceAngle;
            canMoveToNewLine |= Vector3.Angle(inputVector.normalized, intersection.Line.Slope * -1) < toleranceAngle;

            if (intersection.IsParallel)
            {
                canMoveToNewLine &= OnLineController.DistanceOnLine == 0 || OnLineController.DistanceOnLine == 1;
            }

            // Set new line using Intersection Data
            if (canMoveToNewLine)
            {
                SetNewLine(intersection.Line, intersection.DistanceAlongLine);
                break;
            }
        }
    }

    private bool HandleCollisions(Vector2 direction, float distanceToMove)
    {
        bool canMove = true;
        RaycastHit2D[] hits = new RaycastHit2D[2];

        int numberOfHits = _collider.Cast(direction, hits, _pushableDistanceCheck);

        for (int i = 0; i < numberOfHits; i++)
        {
            hits[i].collider.gameObject.TryGetComponent(out LineMovementController pushable);
            hits[i].collider.gameObject.TryGetComponent(out OnLineController onLineController);

            if (onLineController == null)
            {
                continue;
            }

            if (pushable != null)
            {

                Vector2 positionDifference = (Vector2)(onLineController.transform.position - transform.position).normalized;

                if(positionDifference != direction.normalized)
                {
                    continue;
                }

                pushable.SetLevelManager(LevelManager);
                pushable.RecalculateLineInfo();

                // Try to move the pushable
                bool pushableMoved = pushable.MoveAlongLine(direction, distanceToMove);
                canMove = pushableMoved;
            }
            if (onLineController.ObjectType == Enums.ObjectType.UniversalObstacle)
            {
                canMove = false;
            }
            else if (onLineController.ObjectType == Enums.ObjectType.BlockObstacle && OnLineController.ObjectType != Enums.ObjectType.Player)
            {
                canMove = false; 
            }
        }

        return canMove;
    }

    public void RecalculateLineInfo()
    {
        SetNewLine(OnLineController.CurrentLine, OnLineController.DistanceOnLine);
    }

    public void SetLevelManager(LevelManager newLevelManager)
    {
        LevelManager = newLevelManager;
    }

    private void OnGUI()
    {
        if (gameObject.name == "Player")
        {
            GUI.skin.label.fontSize = 24;
            GUILayout.Label("Current Line Slope: " + OnLineController.CurrentLine.Slope);
            GUILayout.Label("Angle Between Input and Slope: " + _angleBetweenInputAndSlope);
        }


    }
}
