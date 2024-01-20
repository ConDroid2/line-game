using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public LineMovementController MovementController;

    public float Speed;
    private bool _allowInput = true;

    [SerializeField] private Transform _visuals;
    
    // Private variables
    BaseControls _baseControls;

    Vector2 _inputSlope = Vector2.zero;

    public System.Action OnPlayerDeath;

    private void Awake()
    {
        _baseControls = new BaseControls();
        _baseControls.PlayerMap.Enable();
    }

    private void Start()
    {
        MovementController.OnTryToMoveInDirection += HandleTryToMoveInDirection;
    }

    private void OnDisable()
    {
        MovementController.OnTryToMoveInDirection -= HandleTryToMoveInDirection;
    }


    // Update is called once per frame
    void Update()
    {

        if (_allowInput)
        {
            _inputSlope = _baseControls.PlayerMap.Move.ReadValue<Vector2>();
            
            if(_inputSlope == Vector2.zero)
            {
                return;
            }
            // Debug.Log(_inputSlope);

            float modifiedSpeed = Speed / MovementController.OnLineController.CurrentLine.Length;
            float distanceThisFrame = modifiedSpeed * Time.deltaTime;

            // Move player
            MovementController.MoveAlongLine(_inputSlope, distanceThisFrame);
        }
    }

    public void SetLevelManager(LevelManager newLevelManager)
    {
        MovementController.SetLevelManager(newLevelManager);
    }

    public void SetNewLine(LineController newLine, float distanceAlongNewLine)
    {
        MovementController.SetNewLine(newLine, distanceAlongNewLine);
    }

    public void GetKilled()
    {
        // Need to send event so level manager can spawn properly
        OnPlayerDeath.Invoke();
    }

    public void HandleTryToMoveInDirection(int direction)
    {
        _visuals.localScale = new Vector3(direction, _visuals.localScale.y, _visuals.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputSlope.x, _inputSlope.y, 0f));
    } 
}
