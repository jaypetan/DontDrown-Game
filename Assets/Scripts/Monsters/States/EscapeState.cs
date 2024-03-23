using UnityEngine;

public class EscapeState : BaseState
{
    private float escapeTimer = 0;
    private float escapeDuration = 10.0f; // Time to spend in escape state
    private float healthRestoreRate = 20f; // Amount of health to restore per second

    public override void Enter()
    {
        enemy.Agent.speed *= 1.5f; // Increase speed to escape faster
        Vector3 escapeDirection = (enemy.transform.position - enemy.Player.transform.position).normalized;
        Vector3 escapeTarget = enemy.transform.position + escapeDirection * enemy.sightDistance * 5f; // Move away to a point half the sightDistance
        enemy.Agent.SetDestination(escapeTarget);
    }

    public override void Perform()
    {
        escapeTimer += Time.deltaTime;

        enemy.RestoreHealth(healthRestoreRate);

        if (escapeTimer >= escapeDuration)
        {
            enemy.Agent.speed /= 1.5f; // Reset speed
            stateMachine.ChangeState(new PatrolState());
        }
    }

    public override void Exit()
    {
        escapeTimer = 0; 
    }
}
