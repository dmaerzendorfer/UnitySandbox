using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateController : MonoBehaviour
{
    private State _currentState;

    [SerializeReference] public List<State> states;

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(states.First());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnStateUpdate();
        }
    }

    public void ChangeState(State newState)
    {
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }

        _currentState = newState;
        _currentState.OnStateEnter(this);
    }
}