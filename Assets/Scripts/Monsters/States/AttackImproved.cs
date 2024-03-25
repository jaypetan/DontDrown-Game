using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackImproved : BaseState
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

            // Calculate direction to the player
            Vector2 direction = enemy.Player.transform.position - enemy.transform.position;

            // Calculate the angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle > 90 || angle < -90)
            {
                // Flip sprite by setting localScale.y to -1
                if (enemy.transform.localScale.y > 0)
                {
                    enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, -enemy.transform.localScale.y, 0);
                }
            }
            else
            {
                // Ensure sprite is not flipped if it doesn't meet the conditions
                if (enemy.transform.localScale.y < 0)
                {
                    enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, -enemy.transform.localScale.y, 0);
                }
            }

            // Apply rotation
            enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            float timeToPlayer = Vector3.Distance(enemy.Player.transform.position, enemy.transform.position) / enemy.Agent.speed;
            if(timeToPlayer > enemy.MovementPredictionTime)
            {
                timeToPlayer = enemy.MovementPredictionTime;
            }
            Vector3 targetPosition = enemy.Player.transform.position + (enemy.playerMovement.AverageVelocity * timeToPlayer);
            Vector2 directionToTarget = (targetPosition - enemy.transform.position).normalized;
            Vector2 directionToPlayer = (enemy.Player.transform.position - enemy.transform.position).normalized;
            float dot = Vector2.Dot(directionToTarget, directionToPlayer);

            if (dot < enemy.MovementPredictionThreshold)
            {
                targetPosition = enemy.Player.transform.position;
            }

            enemy.Agent.SetDestination(targetPosition);

        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > waitBeforeSearchTime)
            {
                // Go back to PatrolState
                stateMachine.ChangeState(new PatrolState());
            }

        }
        if (enemy.curHealth <= 0)
        {
            stateMachine.ChangeState(new EscapeState());
        }
    }
}
