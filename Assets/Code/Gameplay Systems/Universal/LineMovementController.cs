using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMovementController : MonoBehaviour
{
    // External References
    public LineController _currentLine;
    public LevelManager LevelManager { get; private set; }

    // Controlling Variables
    [SerializeField] bool _setUpInfoOnAwake = false;
    public float DistanceAlongLine;
    public float LineDirectionModifier { get; private set; } // Makes it so we can move correctly regardless of slope sign
    public float CheckForIntersectionsDistance; // The distance we check for intersections
    public float Speed;

    private void Awake()
    {
        if (_setUpInfoOnAwake)
        {
            SetDistanceAlongLine(DistanceAlongLine);
        }
    }

    public bool MoveAlongLine(float horizontalInput, float verticalInput)
    {
        Enums.SlopeType inputSlopeType = Utilities.DetermineSlopeType(horizontalInput, verticalInput);

        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(_currentLine, transform.position, CheckForIntersectionsDistance);

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

        // Actually move the object along the line
        // If holding in the positive direction
        if ((_currentLine.SlopeType == Enums.SlopeType.Horizontal && horizontalInput == 1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Vertical && verticalInput == 1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Ascending && horizontalInput == 1 && verticalInput == 1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Descending && horizontalInput == 1 && verticalInput == -1))
        {
           
            DistanceAlongLine = Mathf.Clamp(DistanceAlongLine + (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }
        // If holding int he negative direction
        else if ((_currentLine.SlopeType == Enums.SlopeType.Horizontal && horizontalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Vertical && verticalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Ascending && horizontalInput == -1 && verticalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Descending && horizontalInput == -1 && verticalInput == 1))
        {
            DistanceAlongLine = Mathf.Clamp(DistanceAlongLine - (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }

        bool willMove = Vector3.Lerp(_currentLine.A, _currentLine.B, DistanceAlongLine) != transform.position;
        transform.position = Vector3.Lerp(_currentLine.A, _currentLine.B, DistanceAlongLine);

        return willMove;
    }

    private float GetModifiedSpeed()
    {
        // I think this really only needs to be calculated once when line switcheds
        return (Speed / _currentLine.Length) * LineDirectionModifier;
    }


    // Public Setters
    public void SetNewLine(LineController newLine, float distanceAlongLine = 0)
    {
        _currentLine = newLine;
        SetDistanceAlongLine(distanceAlongLine);

        // Figure out Line Direction Modifier
        if (_currentLine.SlopeType == Enums.SlopeType.Horizontal)
        {
            LineDirectionModifier = _currentLine.Slope.x;
        }
        else if (_currentLine.SlopeType == Enums.SlopeType.Vertical)
        {
            LineDirectionModifier = _currentLine.Slope.y;
        }
        else if (_currentLine.SlopeType == Enums.SlopeType.Ascending || _currentLine.SlopeType == Enums.SlopeType.Descending)
        {
            LineDirectionModifier = _currentLine.Slope.x > 0 ? 1 : -1; // Doesn't matter if we use x or y here since they should be the same
        }
    }

    public void RecalculateLineInfo()
    {
        SetNewLine(_currentLine, DistanceAlongLine);
    }

    public void SetDistanceAlongLine(float newDistance)
    {
        DistanceAlongLine = newDistance;
        transform.position = Vector3.Lerp(_currentLine.A, _currentLine.B, DistanceAlongLine);
    }

    public void SetLevelManager(LevelManager newLevelManager)
    {
        LevelManager = newLevelManager;
    }
}
