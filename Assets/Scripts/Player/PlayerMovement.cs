using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController Controller;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float HistoricalPositionDuration = 1f;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float HistoricalPositionInterval = 0.1f;

    public Vector3 AverageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach(Vector3 velocity in HistoricalVelocities)
            {
                average += velocity;
            }
            average.y = 0;
            return average/HistoricalVelocities.Count;
        }
    }

    private Queue<Vector3> HistoricalVelocities;
    private float LastPositionTime;
    private int MaxQueuesize;

    private void Awake()
    {
        Controller = GetComponent<PlayerController>();
        MaxQueuesize = Mathf.CeilToInt(1f / HistoricalPositionInterval * HistoricalPositionDuration);
        HistoricalVelocities = new Queue<Vector3>(MaxQueuesize);
    }

    private void Update()
    {
        if(LastPositionTime + HistoricalPositionInterval <= Time.time)
        {
            if (HistoricalVelocities.Count > MaxQueuesize)
            {
                HistoricalVelocities.Dequeue();
            }

            HistoricalVelocities.Enqueue(Controller.currentVelocity);
            LastPositionTime = Time.time;
        }
    }
}
