using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Barracuda;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // For debugging
    [SerializeField] private string currentState;

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private GameObject player;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Path path;
    public PlayerMovement playerMovement;

    public float sightDistance = 200f;
    public float fieldOfView = 120f;

    public float maxHealth = 100f;
    public float curHealth;

    public bool canChase = true;
    public bool canDamage = true;
    public bool canStun = false;
    public int enemyDamage = 10;

    public bool useMovementPrediction = false;
    public float WaitDelay = 1f;
    public float MovementPredictionThreshold = 0f;
    [Range(0.25f, 2f)]
    public float MovementPredictionTime = 1f;

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine.Initialise();

        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        curHealth = maxHealth;
    }

    private void Update()
    {
        // Debug.Log(CanSeePlayer());
        currentState = stateMachine.activeState.ToString();
        if (canChase)
        {
            CanSeePlayer();
        }

        // Force Z position to stay at a fixed value, e.g., 0
        var position = transform.position;
        if (position.z != 0)
        {
            transform.position = new Vector3(position.x, position.y, 0);
        }

    }

    public bool CanSeePlayer()
    {
        if (canChase)
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
                        Debug.Log($"Hit: {hit.collider.gameObject.name}");
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
        else
        {
            return false;
        }
    }

    public void TakeDamage(float damage)
    {
        curHealth -= damage;
    }

    public void RestoreHealth(float restoreRate)
    {
        curHealth = Mathf.Min(curHealth + restoreRate * Time.deltaTime, maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

            if (canDamage)
            {
                collision.gameObject.GetComponent<PlayerOxygen>().TakeDamage(enemyDamage);
            }
            if (canStun && playerController != null)
            {
                StartCoroutine(playerController.StunPlayer(3.0f)); // Adjust stun duration as needed
            }
        }
    }

}