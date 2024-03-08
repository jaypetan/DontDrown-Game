using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    private float losePlayerTimer = 0;
    public float waitBeforeSearchTime = 3.0f;
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            enemy.transform.LookAt(enemy.Player.transform);
            enemy.Agent.SetDestination(enemy.transform.position);
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > waitBeforeSearchTime)
            {
                // Go back to PatrolState
                stateMachine.ChangeState(new PatrolState());
            }

        }
    }
}
