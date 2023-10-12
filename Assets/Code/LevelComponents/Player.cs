using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private LineMovementController _movementController;
    
    // Private variables
    float _horizontalInput = 0f;
    float _verticalInput = 0f;

    [SerializeField] private ContactFilter2D _filter;
    [SerializeField] private float _pushableDistanceCheck;

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

        if (_horizontalInput != 0 || _verticalInput != 0)
        {
            // Push moveable things and check if play can move
            bool canMove = HandlePushables();

            // Move player
            if (canMove)
            {
                _movementController.MoveAlongLine(_horizontalInput, _verticalInput);
            }
        }
    }


    private bool HandlePushables()
    {
        bool playerCanMove = true;
        RaycastHit2D[] hits = new RaycastHit2D[2];

        Vector3 raycastDistance = new Vector3(_horizontalInput, _verticalInput).normalized * _pushableDistanceCheck;

        int numberOfHits = Physics2D.Linecast(transform.position, transform.position + raycastDistance, _filter, hits);

        for (int i = 0; i < numberOfHits; i++)
        {
            LineMovementController pushable = hits[i].collider.gameObject.GetComponent<LineMovementController>();

            pushable.Speed = _movementController.Speed;
            pushable.SetLevelManager(_movementController.LevelManager);
            pushable.RecalculateLineInfo();

            // Don't want to move if the pushable didn't for some reason
            bool pushableMoved = pushable.MoveAlongLine(_horizontalInput, _verticalInput);
            playerCanMove = pushableMoved;
        }

        return playerCanMove;
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
}
