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
    public Transform FollowPoint;

    public float Speed;
    public float SprintingSpeed;

    private bool _allowMoving = true;
    [SerializeField] private bool _aimingMode = false;
    private bool _sprinting = false;

    [SerializeField] private Transform _visuals;
    [SerializeField] private Transform _aimPointer;

    // Private variables

    [SerializeField] private bool _shootUnlocked = false;
    [SerializeField] private bool _grappleUnlocked = false;
    [SerializeField] private bool _rotateUnlocked = false;

    [SerializeField] private bool _invulnerable = false;
    private bool _allowInput = true;
    private bool _bufferTurnOffAimMode = false;


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

        // Sprinting Events
        _baseControls.PlayerMap.Sprint.performed += SprintingStatusChanged;
        _baseControls.PlayerMap.Sprint.canceled += SprintingStatusChanged;

        // Aim Mode Events
        _baseControls.PlayerMap.AimMode.performed += AimModeStatusChanged;
        _baseControls.PlayerMap.AimMode.canceled += AimModeStatusChanged;

        // Projectile Ability Events
        _baseControls.PlayerMap.FireProjectile.performed += HandleFirePerformed;

        // Grapple Ability Events
        _baseControls.PlayerMap.Grapple.performed += HandleGrapplePerformed;

        // Rotate Ability Events
        _baseControls.PlayerMap.Rotate.performed += HandleRotatePerformed;

        // Pause Event
        _baseControls.PlayerMap.OpenMenu.performed += HandlePausePerformed;

        _grapplingHook.OnGrappleFinished += HandleGrappleFinished;


    }

    private void Start()
    {
        MovementController.OnTryToMoveInDirection += HandleTryToMoveInDirection;
    }

    private void OnDisable()
    {
        MovementController.OnTryToMoveInDirection -= HandleTryToMoveInDirection;
        _baseControls.PlayerMap.AimMode.performed -= AimModeStatusChanged;
        _baseControls.PlayerMap.AimMode.canceled -= AimModeStatusChanged;
        _baseControls.PlayerMap.Rotate.performed -= HandleRotatePerformed;
        _baseControls.PlayerMap.Grapple.performed -= HandleGrapplePerformed;
        _baseControls.PlayerMap.FireProjectile.performed -= HandleFirePerformed;
        _grapplingHook.OnGrappleFinished -= HandleGrappleFinished;
        _baseControls.PlayerMap.OpenMenu.performed -= HandlePausePerformed;
    }


    // Update is called once per frame
    void Update()
    {
        _inputVector = _baseControls.PlayerMap.Move.ReadValue<Vector2>();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Hitting k");
            GetKilled(Enums.KillType.Default);
        }


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
            // MovementController.MoveAlongLine(_inputVector, distanceThisFrame);

            MovementController.AddForce(new Force(currentSpeed, _inputVector, 0f, gameObject));
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
        // Gate for aiming, need at least one aim ability
        if (_shootUnlocked == false && _grappleUnlocked == false && GameManager.Instance != null) return;

        if(context.phase == InputActionPhase.Performed)
        {
            TurnOnAimMode();
        }

        else if(context.phase == InputActionPhase.Canceled)
        {
            TurnOffAimMode();
        }
    }

    public void HandleFirePerformed(InputAction.CallbackContext context)
    {
        if (_shootUnlocked == false && GameManager.Instance != null) return;

        if (_aimingMode)
        {
            _projectileLauncher.Fire();
        }
    }

    public void HandleGrapplePerformed(InputAction.CallbackContext context)
    {
        if (_grappleUnlocked == false && GameManager.Instance != null) return;

        if (_aimingMode)
        {
            _grapplingHook.AttemptGrapple();
            _allowInput = false;
        }
    }

    public void HandleRotatePerformed(InputAction.CallbackContext context)
    {
        if (_rotateUnlocked == false && GameManager.Instance != null) return;

        // rotate
        // Debug.Log("Rotating");
        MovementController.OnLineController.CurrentLine.GetComponent<LineRotator>().Rotate();
    }

    public void HandlePausePerformed(InputAction.CallbackContext context)
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.HandlePause();
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

    public IEnumerator MakeInvulnerable(float invulnerabilityTime)
    {
        _invulnerable = true;
        float totalTimeInvulnerable = 0f;

        while(totalTimeInvulnerable < invulnerabilityTime)
        {
            totalTimeInvulnerable += Time.deltaTime;
            yield return null;
        }

        _invulnerable = false;
    }

    public void GetKilled(Enums.KillType killType)
    {
        if (_invulnerable == true) return; 
        // Need to send event so level manager can spawn properly
        OnPlayerDeath.Invoke();
        _grapplingHook.FinishGrapple();
        _aimingMode = false;
    }

    public void HandleTryToMoveInDirection(int direction)
    {
        _visuals.localScale = new Vector3(direction, _visuals.localScale.y, _visuals.localScale.z);
    }

    public void UnlockShoot()
    {
        _shootUnlocked = true;
    }

    public void UnlockGrapple()
    {
        _grappleUnlocked = true;
    }

    public void UnlockRotate()
    {
        _rotateUnlocked = true;
    }

    public void HandleGrappleFinished()
    {
        _allowInput = true;

        if(_bufferTurnOffAimMode == true)
        {
            _bufferTurnOffAimMode = false;
            TurnOffAimMode();
        }
    }

    public void TurnOnAimMode()
    {
        _aimingMode = true;
        _allowMoving = false;
        _aimController.Activate();

        if (_bufferTurnOffAimMode == true)
        {
            _bufferTurnOffAimMode = false;
        }
    }

    public void TurnOffAimMode()
    {
        if (_allowInput == false)
        {
            _bufferTurnOffAimMode = true;
        }
        else
        {
            _aimingMode = false;
            _allowMoving = true;
            _aimController.Deactivate();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputVector.x, _inputVector.y, 0f));
    } 
}
