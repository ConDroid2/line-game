using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private LineMovementController _movementController;
    
    // Private variables
    float _horizontalInput = 0f;
    float _verticalInput = 0f;

    Vector2 _inputSlope = Vector2.zero;

    public System.Action OnPlayerDeath;


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

        _inputSlope = new Vector2(_horizontalInput, _verticalInput);

        if (_horizontalInput != 0 || _verticalInput != 0)
        {

            // Move player
            _movementController.MoveAlongLine(_inputSlope);
        }
    }

    public void SetLevelManager(LevelManager newLevelManager)
    {
        _movementController.SetLevelManager(newLevelManager);
    }

    public void SetNewLine(LineController newLine, float distanceAlongNewLine)
    {
        _movementController.SetNewLine(newLine, distanceAlongNewLine);
    }

    public void GetKilled()
    {
        // Need to send event so level manager can spawn properly
        Debug.Log("Player has died");
        OnPlayerDeath.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputSlope.x, _inputSlope.y, 0f).normalized);
    } 
}
