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

    public float sightDistance = 10f;
    public float fieldOfView = 360f;
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
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        Vector3 toPlayer = player.transform.position - transform.position; // Direction to the player
                                                                           // Convert to a 2D vector for 2D angle calculation
        Vector2 toPlayer2D = new Vector2(toPlayer.x, toPlayer.z); // Assuming X and Z are your 2D plane
        Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z); // Enemy's forward direction in 2D

        // Check if within sight distance
        if (toPlayer2D.magnitude <= sightDistance)
        {
            // Calculate angle to see if within field of view
            float angleToPlayer = Vector2.Angle(forward2D, toPlayer2D);

            if (angleToPlayer <= fieldOfView / 2) // Divided by 2 because fieldOfView is the total FOV
            {
                // Optional: Raycast to check for line of sight, ignoring obstacles
                RaycastHit hit;
                if (Physics.Raycast(transform.position, toPlayer.normalized, out hit, sightDistance))
                {
                    if (hit.collider.gameObject == player)
                    {
                        return true; // Player is within sight and no obstacles in between
                    }
                }
            }
        }
        Debug.DrawRay(transform.position, toPlayer.normalized * sightDistance, Color.red);

        return false; // Player is not within sight
    }
}