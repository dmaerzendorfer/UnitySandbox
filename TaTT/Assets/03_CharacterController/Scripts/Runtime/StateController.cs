using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//todo: check if still need this after the new PlayerStateMachine

//todo: create a stats manager 
//this manager holds all the stats scriptable objects
//the stats manager is static and therefore a singleton, so everyone can get their own stats and access other game objects stats as well
//the scriptable object stats themselves decide if they can be changed from outside or not
//
//todo: nvm, a singleton manager makes things worse, makes system more rigid -> wont work without manager
//need to create the SO in the editor either way. scripts can just have SO as property and can also fetch it from other compononents/gameObjects as well.
//in some cases it might be better -> eG if something should half all enemies health forever -> then a single access point is better
//but overall this seems more clean, if really needed it can be implemented then.

/// <summary>
/// A Finite-State-Machine that holds states which are scriptable objects.
/// Can also be used for concurrent FSMs, just give an object multiple stateControllers
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