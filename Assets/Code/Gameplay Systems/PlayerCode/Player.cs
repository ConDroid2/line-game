using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public LineMovementController MovementController;
    [SerializeField] private AimController _aimController;
    [SerializeField] private ProjectileLauncher _projectileLauncher;
    [SerializeField] private GrapplingHook _grapplingHook;

    public float Speed;
    public float SprintingSpeed;

    private bool _allowMoving = true;
    [SerializeField] private bool _aimingMode = false;
    private bool _sprinting = false;

    [SerializeField] private Transform _visuals;
    [SerializeField] private Transform _aimPointer;
    
    // Private variables
    BaseControls _baseControls;

    Vector2 _inputVector = Vector2.zero;

    public System.Action OnPlayerDeath;

    public static Player Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _baseControls = new BaseControls();
        _baseControls.PlayerMap.Enable();

        _baseControls.PlayerMap.Sprint.performed += SprintingStatusChanged;
        _baseControls.PlayerMap.Sprint.canceled += SprintingStatusChanged;

        _baseControls.PlayerMap.AimMode.performed += AimModeStatusChanged;
        _baseControls.PlayerMap.AimMode.canceled += AimModeStatusChanged;

        _baseControls.PlayerMap.FireProjectile.performed += HandleFirePerformed;

        _baseControls.PlayerMap.Grapple.performed += HandleGrapplePerformed;
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
        _inputVector = _baseControls.PlayerMap.Move.ReadValue<Vector2>();


        if (_allowMoving && !_aimingMode)
        {
            
            if(_inputVector == Vector2.zero)
            {
                return;
            }
            // Debug.Log(_inputSlope);

            float currentSpeed = _sprinting ? SprintingSpeed : Speed;

            float modifiedSpeed = currentSpeed / MovementController.OnLineController.CurrentLine.Length;
            float distanceThisFrame = modifiedSpeed * Time.deltaTime;

            // Move player
            MovementController.MoveAlongLine(_inputVector, distanceThisFrame);
        }
        else if (_aimingMode)
        {
            _aimController.SetAim(_inputVector);
        }
    }

    public void SprintingStatusChanged(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            _sprinting = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            _sprinting = false;
        }
    }

    public void AimModeStatusChanged(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            _aimingMode = true;
            _allowMoving = false;
            _aimController.Activate();
        }

        else if(context.phase == InputActionPhase.Canceled)
        {
            _aimingMode = false;
            _allowMoving = true;
            _aimController.Deactivate();
        }
    }

    public void HandleFirePerformed(InputAction.CallbackContext context)
    {
        if (_aimingMode)
        {
            _projectileLauncher.Fire();
        }
    }

    public void HandleGrapplePerformed(InputAction.CallbackContext context)
    {
        if (_aimingMode)
        {
            _grapplingHook.AttemptGrapple();
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
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputVector.x, _inputVector.y, 0f));
    } 
}
