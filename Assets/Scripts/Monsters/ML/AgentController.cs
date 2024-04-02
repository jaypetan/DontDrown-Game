using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.AI;

public class SharkAgent : Agent
{
    public NavMeshAgent navMeshAgent;
    public GameObject player;

    public Vector3 startPosition;

    public float sightDistance = 200f;
    public float fieldOfView = 120f;
    public float maxHealth = 100f;
    public float curHealth;
    public LayerMask playerLayer;
    public LayerMask torchlightLayer;
    private bool isInTorchlight = false;


    public float dodgeReward = 0.1f;
    public float attackReward = 1.0f;
    public float torchlightPenalty = -0.2f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = maxHealth;
        startPosition = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        curHealth = maxHealth;
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
        switch (action)
        {
            case 1: // Attack
                AttackPlayer();
                break;
            case 2: // Dodge
                DodgeTorchlight();
                break;
            default: // Do nothing
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 toPlayer = player.transform.position - transform.position; // 2D Direction to the player

        // Check if within sight distance using 2D distance calculation
        if (toPlayer.magnitude <= sightDistance)
        {
            // Calculate angle to see if within field of view using transform.right as the forward vector in 2D
            float angleToPlayer = Vector2.Angle(transform.right, toPlayer);

            if (angleToPlayer <= fieldOfView / 2) // Check if within field of view
            {
                int layerMask = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast");
                layerMask = ~layerMask; // Invert the layerMask to ignore Enemy and Checkpoint layers
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightDistance, layerMask);


                if (hit.collider != null)
                {
                    // Debug.Log($"Hit: {hit.collider.gameObject.name}");
                    if (hit.collider.CompareTag("Player"))
                    {
                        // Debug.Log("Player Spotted");
                        return true;
                    }
                }
            }
        }

        Debug.DrawRay(transform.position, toPlayer.normalized * sightDistance, Color.red); // Use for debugging
        return false; // Player is not within sight
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Torchlight"))
        {
            isInTorchlight = true;
        }
    }

    private void OnTriggerExit(Collider other)
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

    private void AttackPlayer()
    {
        // Logic for attacking the player
        if (CanSeePlayer())
        {
            // Move towards the player
            navMeshAgent.SetDestination(player.transform.position);
            AddReward(attackReward);
        }
    }

    private void DodgeTorchlight()
    {
        // Logic for dodging torchlight
        if (IsInTorchlight())
        {
            // Move away from the torchlight source or to a safe position
            // This would require knowing where the torchlight is coming from
            AddReward(dodgeReward);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddReward(attackReward); // Reward for colliding with the player (attacking)
            EndEpisode(); // End the episode if the attack is considered a terminal state
        }
    }

    private void FixedUpdate()
    {
        if (IsInTorchlight())
        {
            TakeDamage(); // Apply damage if in torchlight
        }
    }

    private void ResetSharkPosition()
    {
        navMeshAgent.Warp(startPosition); // Resets the position to the start
    }

    private void TakeDamage()
    {
        curHealth -= 10; // Subtract some damage value
        AddReward(torchlightPenalty);

        if (curHealth <= 0)
        {
            ResetSharkPosition();
            curHealth = maxHealth; // Reset health for next episode or continuation
            EndEpisode(); // End the episode if health is zero
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Implement heuristic logic here for manual control if needed
    }
}
