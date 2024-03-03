using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class randomMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionInterval = 0.5f;

    private float nextDirectionChangeTime;
    private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the NPC in the random direction
        transform.Translate(randomDirection * moveSpeed * Time.deltaTime);

        // Check if it's time to change the direction
        if (Time.time >= nextDirectionChangeTime)
        {
            SetRandomDirection();
        }
    }

    void SetRandomDirection()
    {
        // Generate a new random direction
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // Set the next direction change time
        nextDirectionChangeTime = Time.time + changeDirectionInterval;
    }
}
