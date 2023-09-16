using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//based on: https://www.youtube.com/watch?v=GobPch3uCA4&list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy&index=5
public class PlayerStateMachine : MonoBehaviour
{
    /// <summary>
    /// This index needs to match with the joining player from the PlayerConfigurationManager in order to get the playerInput and Stats
    /// </summary>
    public int playerIndex = 0;

    public Animator animator;

    public bool moveRelativeToCamera = true;

    [HideInInspector] private PlayerStats _statsInstance;
    public bool PlayerConnected { get; set; } = false;
    private PlayerControls _controls;

    #region movementMembers

    private bool _isJumping = false;
    private CharacterController _characterController;
    private PlayerControls _playerInput;
    private Vector3 _cameraRelativeMovement;

    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _jumpCountHash;
    private int _isFallingHash;

    private bool _requireNewJumpPress = false;

    private Vector2 _currentMovementInput = Vector2.zero;
    private Vector3 _currentMovement = Vector3.zero;
    private Vector3 _currentRunMovement = Vector3.zero;
    private Vector3 _appliedMovement = Vector3.zero;

    private bool _isMovementPressed;
    private bool _isRunPressed;
    private bool _isJumpPressed = false;
    private int _jumpCount = 0;
    private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    private Coroutine _currentJumpResetRoutine = null;

    #endregion

    //stateMachine
    private BaseState _currentState;
    private PlayerStateFactory _states;

    protected virtual void Awake()
    {
        _controls = new PlayerControls();
        
        _characterController = GetComponent<CharacterController>();

        //setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        //setup animations
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        _isFallingHash = Animator.StringToHash("isFalling");
    }

    private void OnPlayerJoin(int index)
    {
        //if the newly joined player is not us, return
        if (index != playerIndex) return;

        PlayerConnected = true;
        //load our stats
        var playerConfig = PlayerConfigurationManager.Instance.GetPlayerConfig(playerIndex);
        _statsInstance = playerConfig.Stats;
        SetupJumpVariables();

        //could use the players color now too eG to set the models material or smth...

        //setup our input events
        playerConfig.Input.onActionTriggered += OnActionTriggered;
    }

    protected virtual void Start()
    {
        //setup stuff on player join
        PlayerConfigurationManager.Instance.onPlayerJoin.AddListener(OnPlayerJoin);

        //for gravity reasons
        _characterController.Move(_appliedMovement * Time.deltaTime);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if no player has connected yet, we dont have any input
        if (!PlayerConnected) return;
        HandleRotation();
        if (moveRelativeToCamera)
        {
            _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);

            //todo: fix movement speed, right now its also applied to gravity...
            _characterController.Move(_cameraRelativeMovement * (_statsInstance.movementSpeed * Time.deltaTime));
        }
        else
        {
            _characterController.Move(_appliedMovement * (_statsInstance.movementSpeed * Time.deltaTime));
        }

        //this needs to be last or characterControllers isGrounded will be messed up...
        _currentState.UpdateStates();
    }

    protected virtual void OnDestroy()
    {
        PlayerConfigurationManager.Instance.onPlayerJoin.RemoveListener(OnPlayerJoin);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        if (moveRelativeToCamera)
        {
            positionToLookAt.x = _cameraRelativeMovement.x;
            positionToLookAt.z = _cameraRelativeMovement.z;
        }
        else
        {
            positionToLookAt.x = _currentMovement.x;
            positionToLookAt.z = _currentMovement.z;
        }

        positionToLookAt.y = 0f;

        Quaternion currentRotation = transform.rotation;
        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation =
                Quaternion.Slerp(currentRotation, targetRotation,
                    _statsInstance.rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void SetupJumpVariables()
    {
        //the jump is a parabola -> this math results from that.
        //see https://www.youtube.com/watch?v=hG9SzQxaCm8 for detail
        float timeToApex = _statsInstance.maxJumpTime / 2;
        float initialGravity = (-2 * _statsInstance.maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        float initialJumpVelocity = (2 * _statsInstance.maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (_statsInstance.maxJumpHeight + 1)) / Mathf.Pow(timeToApex * 1.175f, 2);
        float secondJumpVelocity = (2 * _statsInstance.maxJumpHeight + 1) / (timeToApex * 1.175f);
        float thirdJumpGravity = (-2 * (_statsInstance.maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float thirdJumpVelocity = (2 * _statsInstance.maxJumpHeight + 2) / (timeToApex * 1.25f);

        _initialJumpVelocities.Add(1, initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, initialGravity);
        _jumpGravities.Add(1, initialGravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    protected virtual void OnActionTriggered(InputAction.CallbackContext obj)
    {
        //the playerConfigs playerInput uses c# events:
        //therefore the action hookup is a bit longer, but also more dynamic (it can be controlled easier in code)

        if (obj.action.name == _controls.CharacterControls.Move.name &&
            (obj.action.phase == InputActionPhase.Started || obj.action.phase == InputActionPhase.Canceled ||
             obj.action.phase == InputActionPhase.Performed))
        {
            OnMovementInput(obj);
        }
        else if (obj.action.name == _controls.CharacterControls.Run.name &&
                 (obj.action.phase == InputActionPhase.Started || obj.action.phase == InputActionPhase.Canceled))
        {
            OnRun(obj);
        }
        else if (obj.action.name == _controls.CharacterControls.Interact1.name &&
                 (obj.action.phase == InputActionPhase.Started || obj.action.phase == InputActionPhase.Canceled))
        {
            OnJump(obj);
        }
    }

    #region InputCallbacks

    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _currentRunMovement.x = _currentMovementInput.x * _statsInstance.runMultiplier;
        _currentRunMovement.z = _currentMovementInput.y * _statsInstance.runMultiplier;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    #endregion

    #region cameraStuff

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        var cameraForwardZProduct = vectorToRotate.z * cameraForward;
        var cameraRightXProduct = vectorToRotate.x * cameraRight;

        var vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    #endregion

    #region getterSetter

    //getters and setters

    public PlayerStats Stats
    {
        get { return _statsInstance; }
        set { _statsInstance = value; }
    }

    public BaseState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }


    public float CurrentMovementY
    {
        get { return _currentMovement.y; }
        set { _currentMovement.y = value; }
    }

    public float AppliedMovementY
    {
        get { return _appliedMovement.y; }
        set { _appliedMovement.y = value; }
    }

    public float AppliedMovementX
    {
        get { return _appliedMovement.x; }
        set { _appliedMovement.x = value; }
    }

    public float AppliedMovementZ
    {
        get { return _appliedMovement.z; }
        set { _appliedMovement.z = value; }
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public bool IsJumping
    {
        get => _isJumping;
        set => _isJumping = value;
    }

    public CharacterController CharacterController
    {
        get => _characterController;
        set => _characterController = value;
    }

    public PlayerControls PlayerInput
    {
        get => _playerInput;
        set => _playerInput = value;
    }

    public int IsWalkingHash
    {
        get => _isWalkingHash;
        set => _isWalkingHash = value;
    }

    public int IsRunningHash
    {
        get => _isRunningHash;
        set => _isRunningHash = value;
    }

    public int IsJumpingHash
    {
        get => _isJumpingHash;
        set => _isJumpingHash = value;
    }

    public int IsFallingHash
    {
        get => _isFallingHash;
        set => _isFallingHash = value;
    }

    public int JumpCountHash
    {
        get => _jumpCountHash;
        set => _jumpCountHash = value;
    }

    public bool RequireNewJumpPress
    {
        get => _requireNewJumpPress;
        set => _requireNewJumpPress = value;
    }

    public Vector2 CurrentMovementInput
    {
        get => _currentMovementInput;
        set => _currentMovementInput = value;
    }

    public Vector3 CurrentMovement
    {
        get => _currentMovement;
        set => _currentMovement = value;
    }

    public Vector3 CurrentRunMovement
    {
        get => _currentRunMovement;
        set => _currentRunMovement = value;
    }

    public Vector3 AppliedMovement
    {
        get => _appliedMovement;
        set => _appliedMovement = value;
    }

    public bool IsMovementPressed
    {
        get => _isMovementPressed;
        set => _isMovementPressed = value;
    }

    public bool IsRunPressed
    {
        get => _isRunPressed;
        set => _isRunPressed = value;
    }

    public int JumpCount
    {
        get => _jumpCount;
        set => _jumpCount = value;
    }

    public Dictionary<int, float> InitialJumpVelocities
    {
        get => _initialJumpVelocities;
        set => _initialJumpVelocities = value;
    }

    public Dictionary<int, float> JumpGravities
    {
        get => _jumpGravities;
        set => _jumpGravities = value;
    }

    public Coroutine CurrentJumpResetRoutine
    {
        get => _currentJumpResetRoutine;
        set => _currentJumpResetRoutine = value;
    }

    public PlayerStateFactory States
    {
        get => _states;
        set => _states = value;
    }

    public bool IsJumpPressed
    {
        get { return _isJumpPressed; }
    }

    #endregion
}