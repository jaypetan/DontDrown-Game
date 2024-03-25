using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AgentController : Agent
{
    public NavMeshAgent agent;
    public GameObject player;
    public Path path;
    private int waypointIndex = 0;

    public float sightDistance = 200f;
    public float escapeDistance = 100f;
    public float safeDistance = 50f;
    public float fieldOfView = 120f;

    public float waitTimer = 1.0f;
    public int enemyDamage = 10;
    public float maxHealth = 100f;
    private float curHealth;

    private PlayerInput playerInput;
    private InputAction patrolAction;
    private InputAction attackAction;
    private InputAction escapeAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.actions.FindActionMap("MLControls");
        patrolAction = actionMap.FindAction("Patrol");
        attackAction = actionMap.FindAction("Attack");
        escapeAction = actionMap.FindAction("Escape");
    }
    public override void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        curHealth = maxHealth;

        agent.updateRotation = false;
    }

    public override void OnEpisodeBegin()
    {
        // Reset health and position if needed
        curHealth = maxHealth;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        sensor.AddObservation(distanceToPlayer);

        // Enemy health
        sensor.AddObservation(curHealth / maxHealth);

        // Player in sight
        sensor.AddObservation(CanSeePlayer() ? 1.0f : 0.0f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        int action = actionBuffers.DiscreteActions[0];
        switch (action)
        {
            case 0:
                Patrol();
                break;
            case 1:
                Attack();
                break;
            case 2:
                Escape();
                break;
            default:
                Debug.Log("Received unknown action: " + action);
                break;
        }
    }

    private void Patrol()
    {
        Vector2 direction = path.waypoints[waypointIndex].position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Subtract 90 to correct for sprite orientation
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);

        // Check if the Z rotation angle is greater than 90 or less than -90 degrees
        if (angle > 90 || angle < -90)
        {
            // Flip sprite by setting localScale.y to -1
            if (transform.localScale.y > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 0);
            }
        }
        else
        {
            // Ensure sprite is not flipped if it doesn't meet the conditions
            if (transform.localScale.y < 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, - transform.localScale.y, 0);
            }
        }

        if (agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;

                AddReward(0.5f);
                agent.SetDestination(path.waypoints[waypointIndex].position);
            }
        }
    }


    private void Attack()
    {
        Debug.Log("Attacking");
        agent.SetDestination(player.transform.position);
        if (CanSeePlayer())
        {
            AddReward(1.0f);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddReward(2.0f);

            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            collision.gameObject.GetComponent<PlayerOxygen>().TakeDamage(enemyDamage);
        }
    }

    private void Escape()
    {
        Debug.Log("Escaping");
        Vector3 escapeDirection = (transform.position - player.transform.position).normalized;
        Vector3 escapeTarget = transform.position + escapeDirection * escapeDistance;
        agent.SetDestination(escapeTarget);

        if (Vector3.Distance(transform.position, player.transform.position) >= safeDistance)
        {
            AddReward(0.5f);
        }
    }

    public bool CanSeePlayer()
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

    private void FixedUpdate()
    {
        CanSeePlayer();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        Debug.Log($"PlayerInput: {playerInput}");
        Debug.Log($"Patrol Action: {patrolAction}");
        Debug.Log($"Attack Action: {attackAction}");
        Debug.Log($"Escape Action: {escapeAction}");

        patrolAction.Enable();
        attackAction.Enable();
        escapeAction.Enable();
    }

    protected override void OnDisable()
    {
        patrolAction.Disable();
        attackAction.Disable();
        escapeAction.Disable();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Assign manual controls for testing different behaviors
        if (patrolAction.IsPressed()) // P for Patrol
        {
            Debug.Log("P pressed");
            discreteActionsOut[0] = 0; 
        }
        else if (attackAction.IsPressed()) // T for Attack
        {
            Debug.Log("T pressed");
            discreteActionsOut[0] = 1; 
        }
        else if (escapeAction.IsPressed()) // E for Escape
        {
            Debug.Log("E pressed");
            discreteActionsOut[0] = 2; 
        }
    }
    void Update()
    {
        var actionBuffers = new ActionBuffers(new ActionSegment<float>(new float[0]), new ActionSegment<int>(new int[1])); // Adjust array sizes as needed for your actions
        Heuristic(in actionBuffers);
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = maxHealth;
            Escape();

            AddReward(-0.5f);

        }
    }
}
