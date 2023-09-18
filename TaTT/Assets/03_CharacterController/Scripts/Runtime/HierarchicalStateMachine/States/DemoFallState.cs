using UnityEngine;

public class DemoFallState : HierarchicalBaseState<DemoPlayerStateFactory>
{
    private DemoPlayerController _controller;

    public DemoFallState(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller) : base(currentContext)
    {
        _controller = controller;
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _controller.Animator.SetBool(_controller.IsFallingHash, true);
    }

    public override void UpdateState()
    {
        HandleGravity();
    }

    public override void ExitState()
    {
        _controller.Animator.SetBool(_controller.IsFallingHash, false);
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

    public void HandleGravity()
    {
        float previousYVelocity = _controller.CurrentMovementY;
        _controller.CurrentMovementY =
            _controller.CurrentMovementY + _controller.StatsInstance.gravity + Time.deltaTime;
        _controller.AppliedMovementY = Mathf.Max((previousYVelocity + _controller.CurrentMovementY) * .5f,
            _controller.StatsInstance.maxFallSpeed);
    }
}