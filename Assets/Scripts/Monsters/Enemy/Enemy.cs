using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Barracuda;
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

    public float sightDistance = 200f;
    public float fieldOfView = 120f;
    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine.Initialise();

        player = GameObject.FindGameObjectWithTag("Player");

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        // Debug.Log(CanSeePlayer());
        currentState = stateMachine.activeState.ToString();
        CanSeePlayer();

        // Force Z position to stay at a fixed value, e.g., 0
        var position = transform.position;
        if (position.z != 0)
        {
            transform.position = new Vector3(position.x, position.y, 0);
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
                int layerMask = 1 << LayerMask.NameToLayer("Enemy");
                layerMask = ~layerMask;

                // Cast a ray towards the player in 2D
                RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightDistance, layerMask);


                if (hit.collider != null)
                {
                    Debug.Log($"Hit: {hit.collider.gameObject.name}");
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.Log("Player Spotted");
                        return true;
                    }
                }
            }
        }

        Debug.DrawRay(transform.position, toPlayer.normalized * sightDistance, Color.red); // Use for debugging
        return false; // Player is not within sight
    }
}