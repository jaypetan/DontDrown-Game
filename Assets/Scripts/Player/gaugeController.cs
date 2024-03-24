using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gaugeController : MonoBehaviour
{
    public Slider sanityMeter;
    public float increaseRate = 1000f;
    public KeyCode increaseKey = KeyCode.Z;

    public Slider cooldownSlider;
    public float cooldownDuration = 5f;
    private float cooldownTimer = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(increaseKey) && cooldownTimer <= 0f)
        {
            IncreaseGauge();
            cooldownTimer = cooldownDuration;         // Start cooldown after press
        }

        // Update the cooldown timer
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

        // Update Cooldown UI
        cooldownSlider.value = cooldownTimer;
    }

    void IncreaseGauge()
    {
        // Calculate new value based on increase rate
        float newValue = sanityMeter.value + increaseRate * Time.deltaTime;

        // Clamp the value to stay within slider's range
        newValue = Mathf.Clamp(newValue, sanityMeter.minValue, sanityMeter.maxValue);

        //Update slider value
        sanityMeter.value = newValue;


    }

}
