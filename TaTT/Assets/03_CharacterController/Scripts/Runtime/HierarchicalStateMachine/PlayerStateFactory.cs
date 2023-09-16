using System.Collections.Generic;

enum PlayerStates
{
    idle,
    walk,
    run,
    grounded,
    jump,
    fall
}

public class PlayerStateFactory
{
    private PlayerStateMachine _context;
    private Dictionary<PlayerStates, BaseState> _states = new Dictionary<PlayerStates, BaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new IdleState(_context, this);
        _states[PlayerStates.walk] = new WalkState(_context, this);
        _states[PlayerStates.run] = new RunState(_context, this);
        _states[PlayerStates.grounded] = new GroundedState(_context, this);
        _states[PlayerStates.jump] = new JumpState(_context, this);
        _states[PlayerStates.fall] = new FallState(_context, this);
    }

    public BaseState Idle()
    {
        return _states[PlayerStates.idle];

    }

    public BaseState Walk()
    {
        return _states[PlayerStates.walk];
    }

    public BaseState Run()
    {
        return _states[PlayerStates.run];
    }

    public BaseState Jump()
    {
        return _states[PlayerStates.jump];
    }

    public BaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }

    public BaseState Fall()
    {
        return _states[PlayerStates.fall];
    }
}