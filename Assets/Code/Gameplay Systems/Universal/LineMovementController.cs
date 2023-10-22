using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovementController : MonoBehaviour
{
    // External References
    // public LineController _currentLine;
    public ObjectOnLine OnLineController;
    public LevelManager LevelManager { get; private set; }

    public float LineDirectionModifier { get; private set; } // Makes it so we can move correctly regardless of slope sign
    public float CheckForIntersectionsDistance; // The distance we check for intersections
    public float Speed;
    [SerializeField] private bool _canPush;
    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private float _pushableDistanceCheck;

    // privates
    private float _angleBetweenInputAndSlope;

    /*
     * @author Connor Davis
     * @description Attempt to move along the line. Return false if didn't move, true if did
     */
    public bool MoveAlongLine(Vector2 inputVector)
    {
        LineController currentLine = OnLineController.CurrentLine;

        // If we get sent a zero vector fory trying to move, set position as same
        if (inputVector == Vector2.zero)
        {
            Debug.Log("Staying still");
            OnLineController.DistanceOnLine = OnLineController.DistanceOnLine;

            return false;
        }

        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(currentLine, transform.position, CheckForIntersectionsDistance);

        foreach (IntersectionData intersection in intersections)
        {
            // Check if the input would allow movment along the intersecting line
            bool canMoveToNewLine = Vector3.Angle(inputVector.normalized.AbsoluteValue(), intersection.Line.Slope.AbsoluteValue()) < 20f;

            // Set new line using Intersection Data
            if (canMoveToNewLine)
            {
                SetNewLine(intersection.Line, intersection.DistanceAlongLine);
                break;
            }
        }

        float newDistanceOnLine = OnLineController.DistanceOnLine;

        _angleBetweenInputAndSlope = Vector3.Angle(inputVector.normalized, currentLine.Slope * LineDirectionModifier);


        // Actually move the object along the line
        // If holding in the positive direction
        if (_angleBetweenInputAndSlope < 90f)
        {
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine + (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }
        // If holding int he negative direction
        else if (_angleBetweenInputAndSlope > 90f)
        {
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine - (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }

        // Check what our new position will be
        Vector3 attemptedNewPosition = OnLineController.CheckNewPosition(newDistanceOnLine);

        bool willMove = attemptedNewPosition != transform.position;

        if (_canPush)
        {
            willMove &= HandleCollisions(inputVector);
        }

        if (willMove)
        {
            OnLineController.DistanceOnLine = newDistanceOnLine;
        }

        return willMove;
    }

    private float GetModifiedSpeed()
    {
        // I think this really only needs to be calculated once when line switcheds
        return (Speed / OnLineController.CurrentLine.Length) * LineDirectionModifier;
    }


    // Public Setters
    public void SetNewLine(LineController newLine, float distanceOnLine = 0)
    {
        // Feel like the direction modifier should be on the line itself, not the line movement controller
        // Figure out Line Direction Modifier
        if (newLine.SlopeType == Enums.SlopeType.Horizontal)
        {
            LineDirectionModifier = newLine.Slope.x;
        }
        else if (newLine.SlopeType == Enums.SlopeType.Vertical)
        {
            LineDirectionModifier = newLine.Slope.y;
        }
        else if (newLine.SlopeType == Enums.SlopeType.Ascending || newLine.SlopeType == Enums.SlopeType.Descending)
        {
            LineDirectionModifier = newLine.Slope.x > 0 ? 1 : -1; // Doesn't matter if we use x or y here since they should be the same
        }

        OnLineController.SetLine(newLine, distanceOnLine);
    }

    private bool HandleCollisions(Vector2 direction)
    {
        bool canMove = true;
        RaycastHit2D[] hits = new RaycastHit2D[2];

        Vector3 raycastDistance = (Vector3)direction.normalized * _pushableDistanceCheck;

        int numberOfHits = Physics2D.Linecast(transform.position, transform.position + raycastDistance, _filter, hits);

        for (int i = 0; i < numberOfHits; i++)
        {
            LineMovementController pushable = hits[i].collider.gameObject.GetComponent<LineMovementController>();

            // Ignore if hit self
            if(pushable == this)
            {
                continue;
            }

            pushable.Speed = Speed;
            pushable.SetLevelManager(LevelManager);
            pushable.RecalculateLineInfo();

            // Try to move the pushable
            bool pushableMoved = pushable.MoveAlongLine(direction);
            canMove = pushableMoved;
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
