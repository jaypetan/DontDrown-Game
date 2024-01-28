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
    private bool isOnCooldown = false;

    public PlayerController playerMovementScript;

    void Start()
    {
        playerMovementScript = GetComponent<PlayerController>();

        if (playerMovementScript == null)
        {
            Debug.LogError("PlayerMovement script not found!");
        }

         // Initialize the slider
        if (boostSlider != null)
        {
            boostSlider.maxValue = cooldownDuration;
            boostSlider.value = 0f;
        }
    }

    void Update()
    {
        // Check for the boost button input
        if (Input.GetKey(KeyCode.X) && cooldownTimer <= 0f)
        {
            cooldownTimer = cooldownDuration;
            // Boost speed when the boost button is pressed
            StartCoroutine(ActivateSpeedBoost(boostedSpeed, speedBoostDuration));
            
        }
        
    }
    
    IEnumerator ActivateSpeedBoost(float speed, float duration)
    {

        // Start cooldown
        StartCooldown();

        // Apply the boosted speed
        ChangeSpeed(speed);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset speed after the boost duration is over
        ChangeSpeed(normalSpeed);

        // End cooldown
        EndCooldown();

        cooldownTimer = 0f;
    }

    void ChangeSpeed(float speed)
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.controlSpeed = speed;
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        StartCoroutine(CooldownCountdown());
    }

    void EndCooldown()
    {
        isOnCooldown = false;
        // Reset cooldown timer
        cooldownTimer = cooldownDuration;
        // Reset UI
        if (boostSlider != null)
        {
            boostSlider.value = 0f;
        }
    }

     IEnumerator CooldownCountdown()
    {
        while (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            if (boostSlider != null)
            {
                boostSlider.value = cooldownDuration - cooldownTimer;
            }
            yield return null;
        }
    }
}
