using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private LineMovementController _movementController;
    
    // Private variables
    BaseControls _baseControls;

    Vector2 _inputSlope = Vector2.zero;

    public System.Action OnPlayerDeath;

    private void Awake()
    {
        _baseControls = new BaseControls();
        _baseControls.PlayerMap.Enable();
    }


    // Update is called once per frame
    void Update()
    {

        _inputSlope = _baseControls.PlayerMap.Move.ReadValue<Vector2>();

        // Move player
        _movementController.MoveAlongLine(_inputSlope);
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
