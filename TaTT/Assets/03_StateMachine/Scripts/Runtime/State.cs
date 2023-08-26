using System;
using UnityEngine;

[Serializable]
public class State
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

    public void OnStateExit()
    {
        //code in here will always be run, no matter what the specific state instance does in its update
        OnExit();
    }

    protected virtual void OnExit()
    {
    }
}