public class DemoIdleState : HierarchicalBaseState<DemoPlayerStateFactory>
{
    private DemoPlayerController _controller;

    public DemoIdleState(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller) : base(currentContext)
    {
        _controller = controller;
    }

    public override void EnterState()
    {
        _controller.Animator.SetBool(_controller.IsWalkingHash, false);
        _controller.Animator.SetBool(_controller.IsRunningHash, false);
        _controller.AppliedMovementX = 0;
        _controller.AppliedMovementZ = 0;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (_controller.IsMovementPressed && _controller.IsRunPressed)
        {
            SwitchState(Ctx.Factory.Run());
        }
        else if (_controller.IsMovementPressed)
        {
            SwitchState(Ctx.Factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
    }
}