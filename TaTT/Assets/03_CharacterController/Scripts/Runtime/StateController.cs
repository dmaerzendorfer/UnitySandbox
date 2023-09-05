using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A Finite-State-Machine that holds states which are scriptable objects.
/// Can also be used for concurrent FSMs, just give an object multiple stateControllers
/// Usage: create a scriptable object that extends the BaseStateSO and implement its methods. Create an instance of the SO, and set it as a member for this script.
/// </summary>
public class StateController : MonoBehaviour
{
    private BaseStateSO _currentState;

    [SerializeField] private List<BaseStateSO> states = new List<BaseStateSO>();
    [HideInInspector] public List<BaseStateSO> stateInstances = new List<BaseStateSO>();

    private void Awake()
    {
        //instantiate the scriptable objects since their can be multiple stateControllers, eG one for player1 and one for player2
        foreach (var state in states)
        {
            stateInstances.Add(Instantiate(state));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(stateInstances.First());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState != null)
        {
            _currentState.OnStateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (_currentState != null)
        {
            _currentState.OnStateFixedUpdate();
        }
    }

    public void ChangeState(BaseStateSO newState)
    {
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }

        _currentState = newState;
        _currentState.OnStateEnter(this);
    }
}