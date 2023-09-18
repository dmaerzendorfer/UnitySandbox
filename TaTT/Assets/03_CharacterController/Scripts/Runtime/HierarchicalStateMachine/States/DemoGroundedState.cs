using UnityEngine;

public class DemoGroundedState : HierarchicalBaseState<DemoPlayerStateFactory>
{
    private DemoPlayerController _controller;

    public DemoGroundedState(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller) : base(currentContext)
    {
        _controller = controller;
        IsRootState = true;
    }

    public override void EnterState()
    {
        InitializeSubState();
        _controller.Animator.gameObject.transform.position = Vector3.zero;
        HandleGravity();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_controller.IsJumpPressed && !_controller.RequireNewJumpPress)
        {
            SwitchState(Ctx.Factory.Jump());
        }
        else if (!_controller.CharacterController.isGrounded)
        {
            SwitchState(Ctx.Factory.Fall());
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
        if (!_controller.PlayerConnected) return;
        _controller.CurrentMovementY = _controller.StatsInstance.gravity;
        _controller.AppliedMovementY = _controller.StatsInstance.gravity;
    }
}