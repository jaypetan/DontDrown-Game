using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour
{
    private float normalSpeed = 5;
    public float boostedSpeed = 10;
    
    public Slider boostSlider;
    public float speedBoostDuration = 2f;

    public float cooldownDuration = 10f;
    private float cooldownTimer = 0f;

    public WASD playerMovementScript;

    void Start()
    {
        playerMovementScript = GetComponent<WASD>();

        if (playerMovementScript == null)
        {
            Debug.LogError("PlayerMovement script not found!");
        }

    }

    void Update()
    {
        // Check for the boost button input
        if (Input.GetKey(KeyCode.X) && cooldownTimer <= 0f)
        {
            // Boost speed when the boost button is pressed
            StartCoroutine(ActivateSpeedBoost(boostedSpeed, speedBoostDuration));
            cooldownTimer = cooldownDuration;
        }
        

        // Update the cooldown timer
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);
        Debug.Log(cooldownTimer);

        // Update Cooldown UI
        boostSlider.value = cooldownTimer;
    }

    IEnumerator ActivateSpeedBoost(float speed, float duration)
    {
        // Apply the boosted speed
        ChangeSpeed(speed);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset speed after the boost duration is over
        ChangeSpeed(normalSpeed);
    }

    void ChangeSpeed(float speed)
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.controlSpeed = speed;
        }
    }


}
