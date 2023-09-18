using System.Collections.Generic;

enum DemoPlayerStates
{
    idle,
    walk,
    run,
    grounded,
    jump,
    fall
}

public class DemoPlayerStateFactory
{
    private HierarchicalStateMachine<DemoPlayerStateFactory> _context;

    private Dictionary<DemoPlayerStates, HierarchicalBaseState<DemoPlayerStateFactory>> _states =
        new Dictionary<DemoPlayerStates, HierarchicalBaseState<DemoPlayerStateFactory>>();

    public DemoPlayerStateFactory(HierarchicalStateMachine<DemoPlayerStateFactory> currentContext,
        DemoPlayerController controller)
    {
        _context = currentContext;
        _states[DemoPlayerStates.idle] = new DemoIdleState(_context, controller);
        _states[DemoPlayerStates.walk] = new DemoWalkState(_context, controller);
        _states[DemoPlayerStates.run] = new DemoRunState(_context, controller);
        _states[DemoPlayerStates.grounded] = new DemoGroundedState(_context, controller);
        _states[DemoPlayerStates.jump] = new DemoJumpState(_context, controller);
        _states[DemoPlayerStates.fall] = new DemoFallState(_context, controller);
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Idle()
    {
        return _states[DemoPlayerStates.idle];
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Walk()
    {
        return _states[DemoPlayerStates.walk];
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Run()
    {
        return _states[DemoPlayerStates.run];
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Jump()
    {
        return _states[DemoPlayerStates.jump];
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Grounded()
    {
        return _states[DemoPlayerStates.grounded];
    }

    public HierarchicalBaseState<DemoPlayerStateFactory> Fall()
    {
        return _states[DemoPlayerStates.fall];
    }
}