using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState
{
    public override void OnStateEnter()
    {
    }

    public override void OnStateExit()
    {
    }

    public override void RunState()
    {
        Patrol();
        // Change state logic
    }

    public void Patrol()
    {
        // Patrol logic
    }
}
