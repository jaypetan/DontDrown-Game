using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Extensions;

public class TrainingPlayerMovement : MonoBehaviour
{
    public GameObject enemy;
    public PlayerController playerController;
    public NavMeshAgent navMeshAgent;
    private AgentOverride2d agentOverride2D;

    [SerializeField]
    private bool isSpeedBoost = false;

    [SerializeField]
    private float distance;

    private float speedBoostDuration = 2f;
    private float speedBoostCooldown = 2f;
    [SerializeField]
    private float currentSpeedBoostTime = 0f;
    [SerializeField]
    private float currentCooldownTime = 0f;
    [SerializeField]
    private bool isBoosting = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerController = GetComponent<PlayerController>();
        agentOverride2D = GetComponent<AgentOverride2d>();

        playerController.isSpeedBoostActive = isSpeedBoost;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsMovementDirection();
        distance = Vector3.Distance(transform.position, enemy.transform.position);

        playerController.isSpeedBoostActive = isSpeedBoost;

        if (enemy != null)
        {
            if (distance >= 5.0f)
            {
                navMeshAgent.SetDestination(enemy.transform.position);
            }
            else
            {
                if (!isBoosting && currentCooldownTime <= 0)
                {
                    StartSpeedBoost();
                }
                else if (isBoosting)
                {
                    currentSpeedBoostTime -= Time.deltaTime;
                    if (currentSpeedBoostTime <= 0)
                    {
                        EndSpeedBoost();
                    }
                }
                else
                {
                    currentCooldownTime -= Time.deltaTime;
                }

                Vector3 directionAwayFromEnemy = (transform.position - enemy.transform.position).normalized;
                Vector3 runAwayPosition = transform.position + directionAwayFromEnemy * 5.0f;
                navMeshAgent.SetDestination(runAwayPosition);
            }
        }
    }

    private void StartSpeedBoost()
    {
        navMeshAgent.speed *= 2;
        currentSpeedBoostTime = speedBoostDuration;
        isBoosting = true;
    }

    private void EndSpeedBoost()
    {
        navMeshAgent.speed /= 2;
        currentCooldownTime = speedBoostCooldown;
        isBoosting = false;
    }

    private void RotateTowardsMovementDirection()
    {
        Vector3 direction = navMeshAgent.desiredVelocity;

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }
}
