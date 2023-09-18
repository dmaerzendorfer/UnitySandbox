using UnityEngine;


public class HierarchicalStateMachine<StateFactory> : MonoBehaviour
{
    protected HierarchicalBaseState<StateFactory> _currentState;

    public HierarchicalBaseState<StateFactory> CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    protected StateFactory _stateFactory;

    public StateFactory Factory
    {
        get => _stateFactory;
        set => _stateFactory = value;
    }

    protected virtual void Awake()
    {
        SetupHierarchicalStateMachine();
    }

    /// <summary>
    /// Initialise any thing that is needed for the statemachine here. This is called in the awake.
    /// </summary>
    protected virtual void SetupHierarchicalStateMachine()
    {
    }

    protected virtual void Update()
    {
        _currentState.UpdateStates();
    }
}