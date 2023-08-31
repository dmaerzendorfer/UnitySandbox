using UnityEngine;

public class BaseStateSO : ScriptableObject
{
    protected StateController sc;

    public void OnStateEnter(StateController stateController)
    {
        sc = stateController;
        OnEnter();
    }

    protected virtual void OnEnter()
    {
    }

    public void OnStateUpdate()
    {
        //code in here will always be run, no matter what the specific state instance does in its update
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
    }
    
    public void OnStateFixedUpdate()
    {
        //code in here will always be run, no matter what the specific state instance does in its update
        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {
    }

    public void OnStateExit()
    {
        //code in here will always be run, no matter what the specific state instance does in its update
        OnExit();
    }

    protected virtual void OnExit()
    {
    }
}
