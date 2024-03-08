using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;
    public float waitTimer;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {

        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public void PatrolCycle()
    {
        Vector2 direction = enemy.path.waypoints[waypointIndex].position - enemy.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Subtract 90 to correct for sprite orientation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRotation, 200 * Time.deltaTime);

        // Check if the Z rotation angle is greater than 90 or less than -90 degrees
        if (angle > 90 || angle < -90)
        {
            // Flip sprite by setting localScale.y to -1
            if (enemy.transform.localScale.y > 0)
            {
                enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, -enemy.transform.localScale.y, enemy.transform.localScale.z);
            }
        }
        else
        {
            // Ensure sprite is not flipped if it doesn't meet the conditions
            if (enemy.transform.localScale.y < 0)
            {
                enemy.transform.localScale = new Vector3(enemy.transform.localScale.x, -enemy.transform.localScale.y, enemy.transform.localScale.z);
            }
        }

        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;

                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);   
            }
        }
    }
}
