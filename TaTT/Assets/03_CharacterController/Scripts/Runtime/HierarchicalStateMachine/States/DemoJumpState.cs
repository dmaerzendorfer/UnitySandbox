using System.Collections;
using UnityEngine;

public class DemoJumpState : HierarchicalBaseState<DemoPlayerStateFactory>
{
    private DemoPlayerController _controller;

    public DemoJumpState(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller) : base(currentContext)
    {
        _controller = controller;
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleJump();
    }

    public override void UpdateState()
    {
        HandleGravity();
    }

    public override void ExitState()
    {
        _controller.Animator.SetBool(_controller.IsJumpingHash, false);
        if (_controller.IsJumpPressed)
        {
            _controller.RequireNewJumpPress = true;
        }

        _controller.CurrentJumpResetRoutine = _controller.StartCoroutine(IJumpResetRoutine());
        if (_controller.JumpCount == 3)
        {
            _controller.JumpCount = 0;
            _controller.Animator.SetInteger(_controller.JumpCountHash, _controller.JumpCount);
        }
    }

    public override void CheckSwitchStates()
    {
        if (_controller.CharacterController.isGrounded)
        {
            SwitchState(Ctx.Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        if (!_controller.IsMovementPressed && !_controller.IsRunPressed)
        {
            SetSubState(Ctx.Factory.Idle());
        }
        else if (_controller.IsMovementPressed && !_controller.IsRunPressed)
        {
            SetSubState(Ctx.Factory.Walk());
        }
        else
        {
            SetSubState(Ctx.Factory.Run());
        }
    }

    private void HandleJump()
    {
        if (_controller.JumpCount < 3 && _controller.CurrentJumpResetRoutine != null)
        {
            Ctx.StopCoroutine(_controller.CurrentJumpResetRoutine);
        }

        _controller.Animator.SetBool(_controller.IsJumpingHash, true);
        _controller.IsJumping = true;
        _controller.JumpCount++;
        _controller.Animator.SetInteger(_controller.JumpCountHash, _controller.JumpCount);
        _controller.CurrentMovementY = _controller.InitialJumpVelocities[_controller.JumpCount];
        _controller.AppliedMovementY = _controller.InitialJumpVelocities[_controller.JumpCount];
    }

    public void HandleGravity()
    {
        bool isFalling = _controller.CurrentMovement.y <= 0.0f || !_controller.IsJumpPressed;
        if (isFalling)
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = _controller.CurrentMovement.y;
            _controller.CurrentMovementY = _controller.CurrentMovement.y +
                                           (_controller.JumpGravities[_controller.JumpCount] *
                                            _controller.StatsInstance.fallMultiplier * Time.deltaTime);
            _controller.AppliedMovementY = Mathf.Max((previousYVelocity + _controller.CurrentMovement.y) * .5f,
                _controller.StatsInstance.maxFallSpeed);
        }
        else
        {
            //previous velocity stuff for framerate consistent jumps -> verlet integration
            float previousYVelocity = _controller.CurrentMovement.y;
            _controller.CurrentMovementY = _controller.CurrentMovement.y +
                                           (_controller.JumpGravities[_controller.JumpCount] * Time.deltaTime);
            _controller.AppliedMovementY = Mathf.Max((previousYVelocity + _controller.CurrentMovement.y) * .5f,
                _controller.StatsInstance.maxFallSpeed);
        }
    }

    private IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(_controller.StatsInstance.comboJumpTimeFrame);
        _controller.JumpCount = 0;
    }
}