using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//based on: https://www.youtube.com/watch?v=GobPch3uCA4&list=PLwyUzJb_FNeQrIxCEjj5AMPwawsw5beAy&index=5
//todo: delete this once its no longer needed
[RequireComponent(typeof(CharacterController))]
public class AnimationAndMovementController : MonoBehaviour
{
    public Animator animator;
    public float rotationFactorPerFrame = 15f;
    public float runMultiplier = 3f;
    public float gravity = -0.8f;
    public float groundedGravity = -0.05f;
    public float maxJumpTime = 0.75f;
    public float maxJumpHeight = 2.0f;
    public float fallMultiplier = 2f;
    public float comboJumpTimeFrame = 0.5f;

    public float maxFallSpeed = -20f;
    //todo: make these jump time and height params a animation curve in the editor -> also the fall multiplier!

    private bool _isJumping = false;

    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _jumpCountHash;

    private bool _isJumpAnimating;

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _appliedMovement;
    
    private bool _isMovementPressed;
    private bool _isRunPressed;
    private bool _isJumpPressed = false;
    private int _jumpCount = 0;
    private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    private Coroutine _currentJumpResetRoutine = null;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();

        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;
        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        //the jump is a parabola -> this math results from that.
        //see https://www.youtube.com/watch?v=hG9SzQxaCm8 for detail
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        float initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * maxJumpHeight + 1) / Mathf.Pow(timeToApex * 1.175f, 2);
        float secondJumpVelocity = (2 * maxJumpHeight + 1) / timeToApex * 1.175f;
        float thirdJumpGravity = (-2 * maxJumpHeight + 2) / Mathf.Pow(timeToApex * 1.25f, 2);
        float thirdJumpVelocity = (2 * maxJumpHeight + 2) / timeToApex * 1.25f;

        _initialJumpVelocities.Add(1, initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpVelocity);
        _initialJumpVelocities.Add(3, thirdJumpVelocity);

        _jumpGravities.Add(0, gravity);
        _jumpGravities.Add(1, gravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }


    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
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
        _currentRunMovement.x = _currentMovementInput.x * runMultiplier;
        _currentRunMovement.z = _currentMovementInput.y * runMultiplier;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void HandleAnimation()
    {
        bool isWalking = animator.GetBool(_isWalkingHash);
        bool isRunning = animator.GetBool(_isRunningHash);

        if (_isMovementPressed && !isWalking)
        {
            animator.SetBool(_isWalkingHash, true);
        }
        else if (!_isMovementPressed && isWalking)
        {
            animator.SetBool(_isWalkingHash, false);
        }

        if ((_isMovementPressed && _isRunPressed) && !isRunning)
        {
            animator.SetBool(_isRunningHash, true);
        }
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
        {
            animator.SetBool(_isRunningHash, false);
        }
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currentMovement.z;

        Quaternion currentRotation = transform.rotation;
        if (_isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation =
                Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
        if (_characterController.isGrounded)
        {
            if (_isJumpAnimating)
            {
                animator.SetBool(_isJumpingHash, false);
                _isJumpAnimating = false;
                _currentJumpResetRoutine = StartCoroutine(JumpResetRoutine());
                if (_jumpCount == 3)
                {
                    _jumpCount = 0;
                    animator.SetInteger(_jumpCountHash, _jumpCount);
                }
            }

            _currentMovement.y = groundedGravity;
            _appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_jumpGravities[_jumpCount] * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f, maxFallSpeed);
        }
        else
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (_jumpGravities[_jumpCount] * Time.deltaTime);
            _appliedMovement.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f, maxFallSpeed);
        }
    }

    private IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(comboJumpTimeFrame);
        _jumpCount = 0;
    }

    private void HandleJump()
    {
        if (!_isJumping && _characterController.isGrounded && _isJumpPressed)
        {
            if (_jumpCount < 3 && _currentJumpResetRoutine != null)
            {
                StopCoroutine(_currentJumpResetRoutine);
            }

            animator.SetBool(_isJumpingHash, true);
            _isJumpAnimating = true;
            _isJumping = true;
            _jumpCount++;
            animator.SetInteger(_jumpCountHash, _jumpCount);
            _currentMovement.y = _initialJumpVelocities[_jumpCount];
            _appliedMovement.y = _initialJumpVelocities[_jumpCount];
        }
        else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleAnimation();
        if (_isRunPressed)
        {
            _appliedMovement.x = _currentRunMovement.x;
            _appliedMovement.z = _currentRunMovement.z;
        }
        else
        {
            
            _appliedMovement.x = _currentMovement.x;
            _appliedMovement.z = _currentMovement.z;
        }
        _characterController.Move(_appliedMovement * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}