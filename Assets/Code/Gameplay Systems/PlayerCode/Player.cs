using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public LineMovementController MovementController;
    [SerializeField] private AimController _aimController;
    [SerializeField] private ProjectileLauncher _projectileLauncher;
    [SerializeField] private GrapplingHook _grapplingHook;
    public Rigidbody2D FollowPoint;

    public float Speed;
    public float SprintingSpeed;
    public float RoomStartInvulnerability;
    public float DelayAfterShoot = 0.5f;

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
    }


    private void Start()
    {
        Debug.Log("Player start");
        if(InputManager.Instance != null)
        {
            _baseControls = InputManager.Instance.Controls;
        }
        else
        {
            _baseControls = new BaseControls();
            _baseControls.PlayerMap.Enable();
        }

        SceneManager.sceneLoaded += HandleSceneLoaded;

        // Sprinting Events
        _baseControls.PlayerMap.Sprint.performed += SprintingStatusChanged;
        _baseControls.PlayerMap.Sprint.canceled += SprintingStatusChanged;

        //Aim Mode Events
        _baseControls.PlayerMap.AimMode.performed += AimModeStatusChanged;
        _baseControls.PlayerMap.AimMode.canceled += AimModeStatusChanged;

        // Projectile Ability Events
        _baseControls.PlayerMap.FireProjectile.performed += HandleFirePerformed;
        _baseControls.PlayerMap.FireProjectile.canceled += HandleFirePerformed;

        // Grapple Ability Events
        _baseControls.PlayerMap.Grapple.performed += HandleGrapplePerformed;
        _baseControls.PlayerMap.Grapple.canceled += HandleGrapplePerformed;

        // Rotate Ability Events
        _baseControls.PlayerMap.Rotate.performed += HandleRotatePerformed;

        _grapplingHook.OnGrappleFinished += HandleGrappleFinished;

        MovementController.OnTryToMoveInDirection += HandleTryToMoveInDirection;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        MovementController.OnTryToMoveInDirection -= HandleTryToMoveInDirection;
        _baseControls.PlayerMap.AimMode.performed -= AimModeStatusChanged;
        _baseControls.PlayerMap.AimMode.canceled -= AimModeStatusChanged;
        _baseControls.PlayerMap.Rotate.performed -= HandleRotatePerformed;
        _baseControls.PlayerMap.Grapple.performed -= HandleGrapplePerformed;
        _baseControls.PlayerMap.Grapple.canceled -= HandleGrapplePerformed;
        _baseControls.PlayerMap.FireProjectile.performed -= HandleFirePerformed;
        _baseControls.PlayerMap.FireProjectile.canceled -= HandleFirePerformed;
        _grapplingHook.OnGrappleFinished -= HandleGrappleFinished;
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
            _grapplingHook.SetPreview();
        }

        
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _visuals.gameObject.SetActive(true);
        _allowMoving = true;
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
        //// Gate for aiming, need at least one aim ability
        //if (_shootUnlocked == false && _grappleUnlocked == false && GameManager.Instance != null) return;

        //if(context.phase == InputActionPhase.Performed)
        //{
        //    TurnOnAimMode();
        //}

        //else if(context.phase == InputActionPhase.Canceled)
        //{
        //    TurnOffAimMode();
        //}
    }

    public void HandleFirePerformed(InputAction.CallbackContext context)
    {
        if (_shootUnlocked == false && GameManager.Instance != null) return;

        if(context.phase == InputActionPhase.Performed)
        {
            TurnOnAimMode();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            if (_aimingMode)
            {
                _projectileLauncher.Fire();
                StartCoroutine(FinishFire());
            }
        }        
    }

    public void HandleGrapplePerformed(InputAction.CallbackContext context)
    {
        if (_grappleUnlocked == false && GameManager.Instance != null) return;

        if (context.phase == InputActionPhase.Performed)
        {
            TurnOnAimMode();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (_aimingMode)
            {
                _grapplingHook.AttemptGrapple();
                _allowInput = false;

                TurnOffAimMode();
            }
        }
    }

    public void HandleRotatePerformed(InputAction.CallbackContext context)
    {
        if (_rotateUnlocked == false && GameManager.Instance != null) return;

        // rotate
        // Debug.Log("Rotating");
        MovementController.OnLineController.CurrentLine.GetComponent<LineRotator>().Rotate();
    }


    public void SetLevelManager(LevelManager newLevelManager)
    {
        MovementController.SetLevelManager(newLevelManager);
    }

    public void SetNewLine(LineController newLine, float distanceAlongNewLine)
    {
        MovementController.SetNewLine(newLine, distanceAlongNewLine);
    }

    /** Coroutines **/
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

    public IEnumerator FinishFire()
    {
        yield return new WaitForSeconds(DelayAfterShoot);
        TurnOffAimMode();
    }

    public void GetKilled(Enums.KillType killType)
    {
        if (_invulnerable == true) return; 
        // Need to send event so level manager can spawn properly
        OnPlayerDeath.Invoke();
        _grapplingHook.FinishGrapple();
        _aimingMode = false;
        _invulnerable = true;
        _allowMoving = false;
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
        TurnOffAimMode();

        //if(_bufferTurnOffAimMode == true)
        //{
        //    _bufferTurnOffAimMode = false;
        //    Debug.Log("Turning off aim mode");
        //    TurnOffAimMode();
        //}
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
