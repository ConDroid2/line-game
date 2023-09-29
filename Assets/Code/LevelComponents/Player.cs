using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _lineDirectionModifier = 1;

    [SerializeField] private float _speed = 0.5f;
    public float CheckForIntersectionsDistance;

    // Private variables
    float _horizontalInput = 0f;
    float _verticalInput = 0f;

    private LineController _currentLine;
    private float _distanceAlongLine = 0f;

    public System.Action OnPlayerDeath;

    // Don't like this but it will do for now
    [HideInInspector] public LevelManager LevelManager;


    // Update is called once per frame
    void Update()
    {
        // Get current horizontal and vertical input
        _horizontalInput = 0f;
        _verticalInput = 0f;

        if (Input.GetKey(KeyCode.RightArrow)) _horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.LeftArrow)) _horizontalInput = -1f;

        if (Input.GetKey(KeyCode.UpArrow)) _verticalInput = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) _verticalInput = -1f;

        Enums.SlopeType inputSlopeType = Utilities.DetermineSlopeType(_horizontalInput, _verticalInput);

        // Check if there are any intersection points in range   
        List<IntersectionData> intersections = LevelManager.GetIntersectionPointsAroundPos(_currentLine, transform.position, CheckForIntersectionsDistance);

        foreach(IntersectionData intersection in intersections)
        {
            // Check if the input would allow movment along the intersecting line
            bool canMoveToNewLine = inputSlopeType == intersection.Line.SlopeType;

            //canMoveToNewLine |= _horizontalInput != 0f && intersection.Line.Slope.y == 0;
            //canMoveToNewLine |= _verticalInput != 0f && intersection.Line.Slope.x == 0;

            // Set new line using Intersection Data
            if (canMoveToNewLine)
            {
                SetNewLine(intersection.Line, intersection.DistanceAlongLine);
                break;
            }
        }

        

        // Actually move the player along the line
        // If holding in the positive direction
        if ((_currentLine.SlopeType == Enums.SlopeType.Horizontal && _horizontalInput == 1) || 
            (_currentLine.SlopeType == Enums.SlopeType.Vertical && _verticalInput == 1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Ascending && _horizontalInput == 1 && _verticalInput == 1)||
            (_currentLine.SlopeType == Enums.SlopeType.Descending && _horizontalInput == 1 && _verticalInput == -1))
        {
            _distanceAlongLine = Mathf.Clamp(_distanceAlongLine + (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }
        // If holding int he negative direction
        else if ((_currentLine.SlopeType == Enums.SlopeType.Horizontal && _horizontalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Vertical && _verticalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Ascending && _horizontalInput == -1 && _verticalInput == -1) ||
            (_currentLine.SlopeType == Enums.SlopeType.Descending && _horizontalInput == -1 && _verticalInput == 1))
        {
            _distanceAlongLine = Mathf.Clamp(_distanceAlongLine - (GetModifiedSpeed() * Time.deltaTime), 0f, 1f);
        }

        transform.position = Vector3.Lerp(_currentLine.A, _currentLine.B, _distanceAlongLine);
    }

    public void SetNewLine(LineController newLine, float distanceAlongNewLine)
    {
        _currentLine = newLine;
        _distanceAlongLine = distanceAlongNewLine;
        
        // Figure out Line Direction Modifier
        if(_currentLine.SlopeType == Enums.SlopeType.Horizontal)
        {
            _lineDirectionModifier = _currentLine.Slope.x;
        }
        else if(_currentLine.SlopeType == Enums.SlopeType.Vertical)
        {
            _lineDirectionModifier = _currentLine.Slope.y;
        }
        else if(_currentLine.SlopeType == Enums.SlopeType.Ascending || _currentLine.SlopeType == Enums.SlopeType.Descending)
        {
            _lineDirectionModifier = _currentLine.Slope.x > 0 ? 1 : -1; // Doesn't matter if we use x or y here since they should be the same
        }
    }


    private float GetModifiedSpeed()
    {
        // I think this really only needs to be calculated once when line switcheds
        return (_speed / _currentLine.Length) * _lineDirectionModifier;
    }

    public void GetKilled()
    {
        // Need to send event so level manager can spawn properly
        Debug.Log("Player has died");
        OnPlayerDeath.Invoke();
    }
}
