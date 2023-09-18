public class DemoRunState : HierarchicalBaseState<DemoPlayerStateFactory>
{
    private DemoPlayerController _controller;

    public DemoRunState(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller) : base(currentContext)
    {
        _controller = controller;
    }

    public override void EnterState()
    {
        _controller.Animator.SetBool(_controller.IsWalkingHash, true);
        _controller.Animator.SetBool(_controller.IsRunningHash, true);
    }

    public override void UpdateState()
    {
        _controller.AppliedMovementX = _controller.CurrentMovementInput.x * _controller.StatsInstance.runMultiplier;
        _controller.AppliedMovementZ = _controller.CurrentMovementInput.y * _controller.StatsInstance.runMultiplier;
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!_controller.IsMovementPressed)
        {
            SwitchState(Ctx.Factory.Idle());
        }
        else if (_controller.IsMovementPressed && !_controller.IsRunPressed)
        {
            SwitchState(Ctx.Factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
    }
}