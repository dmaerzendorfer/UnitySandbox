public abstract class BaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private BaseState _currentSubState;
    private BaseState _currentSuperState;

    protected bool IsRootState
    {
        set { _isRootState = value; }
    }

    protected PlayerStateMachine Ctx
    {
        get { return _ctx; }
    }

    protected PlayerStateFactory Factory
    {
        get { return _factory; }
    }


    public BaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    /// <summary>
    /// Called as soon as this state is entered.
    /// </summary>
    public abstract void EnterState();

    /// <summary>
    /// Called once per frame before checking if a switch of states is needed.
    /// </summary>
    public abstract void UpdateState();

    private void UpdateCycle()
    {
        UpdateState();
        CheckSwitchStates();
    }

    public void UpdateStates()
    {
        UpdateCycle();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    /// <summary>
    /// Called when this state is exited.
    /// </summary>
    public abstract void ExitState();

    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    /// <summary>
    /// Called after the update. For checking if another state should be entered.
    /// </summary>
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    /// <summary>
    /// switches to a new state
    /// </summary>
    /// <param name="newState"></param>
    protected void SwitchState(BaseState newState)
    {
        ExitState();
        newState.EnterState();
        if (_isRootState)
        {
            //switch current state of context
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            //set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(BaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}