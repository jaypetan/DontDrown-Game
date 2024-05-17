using UnityEngine;

public class OctopusArmController : MonoBehaviour
{
    private Animator animator;
    public float minTime = 5.0f; // Minimum time between arm rises
    public float maxTime = 20.0f; // Maximum time between arm rises
    public float armUpDuration = 2.0f; // Duration the arm stays up

    private float timer;
    private float nextTime;
    private bool armUp = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        SetNextTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!armUp && timer >= nextTime)
        {
            RiseArm();
        }

        if (armUp && timer >= armUpDuration)
        {
            LowerArm();
        }

    }

    void SetNextTime()
    {
        timer = 0;
        nextTime = Random.Range(minTime, maxTime);
    }

    void RiseArm()
    {
        animator.SetTrigger("Rise");
        armUp = true;
        timer = 0; // Reset timer for the duration the arm stays up
    }

    void LowerArm()
    {
        animator.ResetTrigger("Rise");
        armUp = false;
        SetNextTime(); // Reset the timer for the next rise
    }

}