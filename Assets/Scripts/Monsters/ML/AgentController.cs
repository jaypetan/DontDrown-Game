using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SharkAgent : Agent
{
    public NavMeshAgent navMeshAgent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Path path;

    private Vector3 startPosition;
    private float sightDistance = 30f;
    private float fieldOfView = 120f;
    private float maxHealth = 100f;
    private float curHealth;
    private LayerMask playerLayer;
    private LayerMask torchlightLayer;
    private bool isInTorchlight = false;

    private int waypointIndex;
    private float waitTimer;

    private float dodgeReward = 0.1f;
    private float attackReward = 1.0f;
    private float torchlightPenalty = -0.2f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        curHealth = maxHealth;
        startPosition = transform.position;
        navMeshAgent.updateRotation = false; // Prevent automatic rotation updates
        navMeshAgent.updateUpAxis = false;
    }

    public override void OnEpisodeBegin()
    {
        curHealth = maxHealth;
        ResetSharkPosition();
        Debug.Log("Episode started, resetting shark position and health. Current Health: " + curHealth);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(transform.position, player.transform.position)); // Player distance
        sensor.AddObservation(curHealth / maxHealth); // Normalized health
        sensor.AddObservation(CanSeePlayer() ? 1.0f : 0.0f); // Player visibility
        sensor.AddObservation(IsInTorchlight() ? 1.0f : 0.0f); // Whether the shark is in torchlight
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int action = actionBuffers.DiscreteActions[0];
        // Debug.Log($"Action received: {action}");
        switch (action)
        {
            case 1:
                Patrol();
                break;
            case 2:
                AttackPlayer();
                break;
            case 3:
                Escape();
                break;
            default:
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 toPlayer = player.transform.position - transform.position; // 2D Direction to the player

        if (toPlayer.magnitude <= sightDistance)
        {
            float angleToPlayer = Vector2.Angle(transform.right, toPlayer);

            if (angleToPlayer <= fieldOfView / 2)
            {
                int layerMask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast"));
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightDistance, layerMask);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }

        Debug.DrawRay(transform.position, toPlayer.normalized * sightDistance, Color.red);
        return false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Torchlight"))
        {
            isInTorchlight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Torchlight"))
        {
            isInTorchlight = false;
        }
    }

    private bool IsInTorchlight()
    {
        return isInTorchlight;
    }

    private void Patrol()
    {
        if (path == null || path.waypoints.Count == 0)
        {
            return;
        }

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            waypointIndex = (waypointIndex + 1) % path.waypoints.Count;
            navMeshAgent.SetDestination(path.waypoints[waypointIndex].position);
            Debug.Log($"Patrolling to waypoint {waypointIndex}");
        }
    }

    private void AttackPlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        AddReward(attackReward);
        Debug.Log("Attacking player");
    }

    private void Escape()
    {
        Vector2 escapeDirection = transform.position - player.transform.position;
        Vector2 escapePosition = (Vector2)transform.position + escapeDirection.normalized * sightDistance;
        navMeshAgent.SetDestination(escapePosition);
        AddReward(dodgeReward);
        Debug.Log("Escaping from torchlight");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddReward(attackReward);
            Debug.Log("Collided with player, ending episode");
            EndEpisode();
        }
    }

    private void FixedUpdate()
    {
        if (IsInTorchlight())
        {
            Debug.Log("Shark is in torchlight, taking damage");
            TakeDamage();
        }
    }

    private void ResetSharkPosition()
    {
        navMeshAgent.Warp(startPosition);
        Debug.Log("Resetting shark position to " + startPosition);
    }

    private void TakeDamage()
    {
        curHealth -= 10;
        AddReward(torchlightPenalty);
        Debug.Log("Taking damage, current health: " + curHealth);

        if (curHealth <= 0)
        {
            Debug.Log("Health dropped to zero, ending episode");
            ResetSharkPosition();
            curHealth = maxHealth;
            SetReward(-10);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (CanSeePlayer())
        {
            discreteActionsOut[0] = 2; // Attack
        }
        else if (curHealth < maxHealth * 0.3f)
        {
            discreteActionsOut[0] = 3; // Escape
        }
        else
        {
            discreteActionsOut[0] = 1; // Patrol
        }

        // Debug.Log($"Heuristic action set to: {discreteActionsOut[0]}");
    }
}
