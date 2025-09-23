using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class Player : MonoBehaviour
{

    [SerializeField] private PlayerVisualsController _visualsController;
    public LineMovementController MovementController;
    [SerializeField] private AimController _aimController;
    [SerializeField] private ProjectileLauncher _projectileLauncher;
    [SerializeField] private GrapplingHook _grapplingHook;
    public Rigidbody2D FollowPoint;

    public float Speed;
    public float SprintingSpeed;
    public float RoomStartInvulnerability;
    public bool NoFailMode = false;
    public float DelayAfterShoot = 0.5f;

    private bool _allowMoving = true;
    private bool _dying = false;
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

    [SerializeField] private Animator _eyeAnimator;


    BaseControls _baseControls;

    Vector2 _inputVector = Vector2.zero;

    // Events
    public UnityEvent OnPlayerDeath;
    public UnityEvent OnGrapple;
    public UnityEvent OnRotate;

    public static Player Instance = null;

    public enum AbilityEnum { Grapple, Shoot, Default };
    private AbilityEnum _currentAbilityInUse = AbilityEnum.Default;

    // Debug Values
    public Vector2 PlayerInput => _inputVector;

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

        // Projectile Ability Events
        _baseControls.PlayerMap.FireProjectile.performed += HandleFirePerformed;
        _baseControls.PlayerMap.FireProjectile.canceled += HandleFirePerformed;

        // Grapple Ability Events
        _baseControls.PlayerMap.Grapple.performed += HandleGrapplePerformed;
        _baseControls.PlayerMap.Grapple.canceled += HandleGrapplePerformed;

        // Rotate Ability Events
        _baseControls.PlayerMap.Rotate.performed += HandleRotatePerformed;

        // Self Destruct Events
        _baseControls.PlayerMap.SelfDestruct.started += HandleSelfDestructPerformed;
        _baseControls.PlayerMap.SelfDestruct.canceled += HandleSelfDestructPerformed;
        _baseControls.PlayerMap.SelfDestruct.performed += HandleSelfDestructPerformed;

        _grapplingHook.OnGrappleFinished += HandleGrappleFinished;

        MovementController.OnTryToMoveInDirection += HandleTryToMoveInDirection;
    }

    private void OnDisable()
    {
        Debug.Log("Player disabled");
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        MovementController.OnTryToMoveInDirection -= HandleTryToMoveInDirection;
        _baseControls.PlayerMap.Rotate.performed -= HandleRotatePerformed;
        _baseControls.PlayerMap.Grapple.performed -= HandleGrapplePerformed;
        _baseControls.PlayerMap.Grapple.canceled -= HandleGrapplePerformed;
        _baseControls.PlayerMap.FireProjectile.performed -= HandleFirePerformed;
        _baseControls.PlayerMap.FireProjectile.canceled -= HandleFirePerformed;
        _baseControls.PlayerMap.SelfDestruct.started -= HandleSelfDestructPerformed;
        _baseControls.PlayerMap.SelfDestruct.canceled -= HandleSelfDestructPerformed;
        _baseControls.PlayerMap.SelfDestruct.performed -= HandleSelfDestructPerformed;
        _grapplingHook.OnGrappleFinished -= HandleGrappleFinished;
    }


    // Update is called once per frame
    void Update()
    {
        _inputVector = _baseControls.PlayerMap.Move.ReadValue<Vector2>();

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.Log("Hitting k");
        //    GetKilled(Enums.KillType.Default);
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _eyeAnimator.SetTrigger("Left_Right_Blink");
        //}

        if (_allowMoving && !_aimingMode)
        {
            
            if(_inputVector == Vector2.zero)
            {
                return;
            }
            
            Debug.Log(_inputVector);

            float currentSpeed = _sprinting ? SprintingSpeed : Speed;

            float modifiedSpeed = currentSpeed / MovementController.OnLineController.CurrentLine.Length;
            float distanceThisFrame = modifiedSpeed * Time.deltaTime;

            // Move player
            // MovementController.MoveAlongLine(_inputVector, distanceThisFrame);
            float movementDrag = 0f;
            MovementController.AddForce(new Force(currentSpeed, _inputVector, movementDrag, gameObject));
        }
        else if (_aimingMode)
        {
            _inputVector = new Vector2(LimitDirectionalAxis(_inputVector.x), LimitDirectionalAxis(_inputVector.y));
            if (_inputVector != Vector2.zero)
            {
                _aimController.SetAim(_inputVector);
            }
            
            if(_grapplingHook.enabled)
                _grapplingHook.SetPreview();
        }

        
    }

    public float LimitDirectionalAxis(float axisValue)
    {
        if (axisValue < -0.5) return -1;
        else if (axisValue > 0.5) return 1;
        return 0;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _visuals.gameObject.SetActive(true);
        _allowMoving = true;
        _dying = false;
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

    public void HandleFirePerformed(InputAction.CallbackContext context)
    {
        if (_shootUnlocked == false && GameManager.Instance != null) return;
        if (_aimingMode && _currentAbilityInUse != AbilityEnum.Shoot) return;

        if(context.phase == InputActionPhase.Performed)
        {
            TurnOnAimMode(AbilityEnum.Shoot);
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
        if (_aimingMode && _currentAbilityInUse != AbilityEnum.Grapple) return;
        if (_dying == true) return;

        if (context.phase == InputActionPhase.Performed)
        {
            TurnOnAimMode(AbilityEnum.Grapple);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (_aimingMode)
            {
                _grapplingHook.AttemptGrapple();
                _allowInput = false;
                OnGrapple?.Invoke();
                TurnOffAimMode();
            }
        }
    }

    public void HandleRotatePerformed(InputAction.CallbackContext context)
    {
        if (_rotateUnlocked == false && GameManager.Instance != null) return;
        if (_currentAbilityInUse != AbilityEnum.Default) return;

        // rotate
        // Debug.Log("Rotating");
        MovementController.OnLineController.CurrentLine.GetComponent<LineRotator>().Rotate();
        OnRotate?.Invoke();
    }

    public void HandleSelfDestructPerformed(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            _visualsController.HandleSelfDestructStarted(_baseControls);
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            _visualsController.HandleSelfDestructCompleted();
        }
        else if(context.phase == InputActionPhase.Performed)
        {
            _visualsController.HandleSelfDestructCompleted();
            GetKilled(Enums.KillType.SelfDestruct);
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
        _allowInput = true;
        TurnOffAimMode();
    }

    public void GetKilled(Enums.KillType killType)
    {
        if (_invulnerable == true) return;

        if (NoFailMode && killType != Enums.KillType.SelfDestruct) return;
        // Need to send event so level manager can spawn properly
        OnPlayerDeath.Invoke();
        _grapplingHook.FinishGrapple();
        _aimingMode = false;
        _invulnerable = true;
        _allowMoving = false;
        _dying = true;

        MovementController.ClearLineSwapData();
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

    public void TurnOnAimMode(AbilityEnum abilityUsed)
    {
        _aimingMode = true;
        _allowMoving = false;
        _aimController.Activate(abilityUsed);
        _currentAbilityInUse = abilityUsed;

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
            _currentAbilityInUse = AbilityEnum.Default;
        }
    }

    public void SetNoFailMode(bool noFailMode)
    {
        NoFailMode = noFailMode;
    }

    public void SetAllowMove(bool allowMoving)
    {
        Debug.Log($"Setting Player allow moving: {allowMoving}");
        _allowMoving = allowMoving;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_inputVector.x, _inputVector.y, 0f));
    } 
}
