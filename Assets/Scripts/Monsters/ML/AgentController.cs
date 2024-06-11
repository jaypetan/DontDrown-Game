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
    [SerializeField]
    private GameObject torch;

    private Vector3 startPosition;
    private Vector3 playerStartPosition;
    [SerializeField]
    private float sightDistance = 10f;
    [SerializeField]
    private float fieldOfView = 200f;
    private float maxHealth = 100f;
    [SerializeField]
    private float curHealth;
    private LayerMask playerLayer;
    private LayerMask torchlightLayer;
    private bool isInTorchlight = false;

    private int waypointIndex;
    private float waitTimer;

    [SerializeField]
    private float surviveEverySecondReward = 0.5f;
    [SerializeField]
    private float torchlightReward= -0.5f;
    [SerializeField]
    private float deathReward = -50f;
    [SerializeField]
    private float hitPlayerRewards = 10f;
    [SerializeField]
    private float patrolReward = 1f;

    private float damageTickRate = 0.5f; // Time in seconds between damage ticks
    private float timeSinceLastDamage = 0f;
    private float timeSinceLastAttack = 0f;
    private float AttackRate = 0.5f;
    private float timer = 0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        curHealth = maxHealth;
        startPosition = transform.position;
        playerStartPosition = player.transform.position;
        navMeshAgent.updateRotation = false; // Prevent automatic rotation updates
        navMeshAgent.updateUpAxis = false;   // Ensure the NavMeshAgent does not update the up axis
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
        sensor.AddObservation(Vector3.Distance(transform.position, torch.transform.position)); // Torch distance
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
        // Debug.Log("Checking if can see player. ToPlayer vector: " + toPlayer);

        if (toPlayer.magnitude <= sightDistance)
        {
            float angleToPlayer = Vector2.Angle(transform.right, toPlayer);
            // Debug.Log("Angle to player: " + angleToPlayer);

            if (angleToPlayer <= fieldOfView / 2)
            {
                int layerMask = ~(1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ignore Raycast"));
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightDistance, layerMask);
                Debug.DrawRay(transform.position, toPlayer.normalized * sightDistance, Color.red);

                if (hit.collider != null)
                {
                    // Debug.Log("Raycast hit: " + hit.collider.name);
                    if (hit.collider.CompareTag("Player"))
                    {
                        // Debug.Log("Player is in sight");
                        return true;
                    }
                }
                else
                {
                    // Debug.Log("Raycast did not hit any collider.");
                }
            }
        }

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

        // Calculate the direction and rotation to face the waypoint
        Vector2 direction = path.waypoints[waypointIndex].position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Subtract 90 to correct for sprite orientation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);

        // Set the new destination
        if (navMeshAgent.remainingDistance < 0.5f && !navMeshAgent.pathPending)
        {
            waypointIndex = (waypointIndex + 1) % path.waypoints.Count;
            navMeshAgent.SetDestination(path.waypoints[waypointIndex].position);
            // Debug.Log($"Patrolling to waypoint {waypointIndex}");

            AddReward(patrolReward);
        }

        if (angle > 90 || angle < -90)
        {
            // Flip sprite by setting localScale.y to -1
            if (transform.localScale.y > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
            // Ensure sprite is not flipped if it doesn't meet the conditions
            if (transform.localScale.y < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void AttackPlayer()
    {
        if (CanSeePlayer())
        {
            Debug.Log("Attacking player");
            navMeshAgent.SetDestination(player.transform.position);

            // Calculate the direction and rotation to face the waypoint
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Subtract 90 to correct for sprite orientation
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);

            if (angle > 90 || angle < -90)
            {
                // Flip sprite by setting localScale.y to -1
                if (transform.localScale.y > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
            }
            else
            {
                // Ensure sprite is not flipped if it doesn't meet the conditions
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                }
            }
        }
        
    }

    private void Escape()
    {
        Vector2 escapeDirection = transform.position - player.transform.position;
        Vector2 escapePosition = (Vector2)transform.position + escapeDirection.normalized * sightDistance;
        navMeshAgent.SetDestination(escapePosition);
        curHealth += Time.deltaTime * 15;
        Debug.Log("Escaping from torchlight");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            timeSinceLastAttack += Time.deltaTime;
            AddReward(hitPlayerRewards);
            if (timeSinceLastAttack >= AttackRate)
            {
                Debug.Log("Collided with player 10 times, ending episode");
                timeSinceLastAttack = 0f;
                EndEpisode();

            }
        }
    }

    private void FixedUpdate()
    {
        timeSinceLastDamage += Time.fixedDeltaTime;

        if (IsInTorchlight())
        {
            Debug.Log("Shark is in torchlight, taking damage");
            if (timeSinceLastDamage >= damageTickRate)
            {
                TakeDamage();
                timeSinceLastDamage = 0f; // Reset the timer
            }
        }

        timer += Time.fixedDeltaTime;
        if(timer >= 1)
        {
            AddReward(surviveEverySecondReward);
            timer = 0;
        }
    }

    private void ResetSharkPosition()
    {
        navMeshAgent.Warp(startPosition);
        player.transform.position = playerStartPosition;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Reset to default 2D rotation
        // Debug.Log("Resetting shark position to " + startPosition);
    }

    private void TakeDamage()
    {
        curHealth -= 10;
        AddReward(torchlightReward);
        Debug.Log("Taking damage, current health: " + curHealth);

        if (curHealth <= 0)
        {
            Debug.Log("Health dropped to zero, ending episode");
            ResetSharkPosition();
            curHealth = maxHealth;
            SetReward(deathReward);
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
    }
}
