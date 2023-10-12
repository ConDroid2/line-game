using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovementController : MonoBehaviour
{
    // External References
    // public LineController _currentLine;
    public ObjectOnLine OnLineController;
    public LevelManager LevelManager { get; private set; }

    // Controlling Variables
    [SerializeField] bool _setUpInfoOnAwake = false;
    // public float DistanceAlongLine;
    public float LineDirectionModifier { get; private set; } // Makes it so we can move correctly regardless of slope sign
    public float CheckForIntersectionsDistance; // The distance we check for intersections
    public float Speed;

    public bool MoveAlongLine(float horizontalInput, float verticalInput)
    {
        LineController currentLine = OnLineController.CurrentLine;
        Enums.SlopeType inputSlopeType = Utilities.DetermineSlopeType(horizontalInput, verticalInput);

        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(currentLine, transform.position, CheckForIntersectionsDistance);

        foreach (IntersectionData intersection in intersections)
        {
            // Check if the input would allow movment along the intersecting line
            bool canMoveToNewLine = inputSlopeType == intersection.Line.SlopeType;

            // Set new line using Intersection Data
            if (canMoveToNewLine)
            {
                SetNewLine(intersection.Line, intersection.DistanceAlongLine);
                break;
            }
        }

        float newDistanceOnLine = OnLineController.DistanceOnLine;

        // Actually move the object along the line
        // If holding in the positive direction
        if ((currentLine.SlopeType == Enums.SlopeType.Horizontal && horizontalInput == 1) ||
            (currentLine.SlopeType == Enums.SlopeType.Vertical && verticalInput == 1) ||
            (currentLine.SlopeType == Enums.SlopeType.Ascending && horizontalInput == 1 && verticalInput == 1) ||
            (currentLine.SlopeType == Enums.SlopeType.Descending && horizontalInput == 1 && verticalInput == -1))
        {
           
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine + (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }
        // If holding int he negative direction
        else if ((currentLine.SlopeType == Enums.SlopeType.Horizontal && horizontalInput == -1) ||
                 (currentLine.SlopeType == Enums.SlopeType.Vertical && verticalInput == -1) ||
                 (currentLine.SlopeType == Enums.SlopeType.Ascending && horizontalInput == -1 && verticalInput == -1) ||
                 (currentLine.SlopeType == Enums.SlopeType.Descending && horizontalInput == -1 && verticalInput == 1))
        {
            newDistanceOnLine = Mathf.Clamp(OnLineController.DistanceOnLine - (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }

        Vector3 attemptedNewPosition = OnLineController.CheckNewPosition(newDistanceOnLine);

        bool willMove = attemptedNewPosition != transform.position;

        if (willMove)
        {
            OnLineController.DistanceOnLine = newDistanceOnLine;
        }

        return willMove;
    }

    public bool HandleCollisions()
    {
        bool objectCanMove = true;

        return objectCanMove;
    }

    private float GetModifiedSpeed()
    {
        // I think this really only needs to be calculated once when line switcheds
        return (Speed / OnLineController.CurrentLine.Length) * LineDirectionModifier;
    }


    // Public Setters
    public void SetNewLine(LineController newLine, float distanceOnLine = 0)
    {
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

    public void RecalculateLineInfo()
    {
        SetNewLine(OnLineController.CurrentLine, OnLineController.DistanceOnLine);
    }

    public void SetLevelManager(LevelManager newLevelManager)
    {
        LevelManager = newLevelManager;
    }
}
