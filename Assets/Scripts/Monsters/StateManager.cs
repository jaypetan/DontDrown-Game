using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private EnemyState activeState;

    public void Initialise()
    {
        
    }
    void Start()
    {
    }

    void Update()
    {
        if(activeState != null)
        {
            activeState.OnStateEnter();
        } 
    }

    public void SwitchState(EnemyState newState)
    {
        if (activeState != null)
        {
            // run cleanup on activeState
            activeState.OnStateExit();
        }

        // cange to a new state
        activeState = newState;

        // fail-safe null check to make sure new state wasn't null
        if (activeState != null)
        {
            // Setup new state
            activeState.stateManager = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.OnStateEnter();
        }
    }
}
