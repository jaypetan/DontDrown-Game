using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanity : MonoBehaviour
{
    public int maxSanity = 100;
    public int curSanity;
    public int sanityDecreaseRate = 2;
    bool sanityDecreaseZone = true;
    float elapsed = 0f;
    public GaugeUI GaugeUI;
    public Slider sanityMeter;
    public float increaseRate = 1000f;
    public KeyCode increaseKey = KeyCode.Z;

    public Slider cooldownSlider;
    public float cooldownDuration = 3f;
    private float cooldownTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        curSanity = maxSanity;
        SetSanity();
    }

    void SetSanity()
    {
        GaugeUI.SetInitialGauge(maxSanity);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= 1f && sanityDecreaseZone)
        {
            elapsed = elapsed % 1f;
            sanityDecrease();
        }

        if (Input.GetKey(increaseKey) && cooldownTimer <= 0f)
        {
            IncreaseGauge();
            // Start cooldown after press
            cooldownTimer = cooldownDuration;         
        }

        // Update the cooldown timer
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

        // Update Cooldown UI
        cooldownSlider.value = cooldownTimer;
    }

    private void sanityDecrease()
    {
        curSanity -= sanityDecreaseRate;
        curSanity = Mathf.Clamp(curSanity, 0, maxSanity);
        GaugeUI.UpdateGauge(curSanity);

    }

    public void IncreaseGauge()
    {
        // Calculate new value based on increase rate
        float newValue = curSanity + increaseRate * Time.deltaTime;

        // Clamp the value to stay within slider's range
        newValue = Mathf.Clamp(newValue, sanityMeter.minValue, sanityMeter.maxValue);

        //Update slider value
        curSanity = (int)newValue;
        curSanity = Mathf.Clamp(curSanity, 0, maxSanity);
        GaugeUI.UpdateGauge(curSanity);
    }

    public void TakeSanityDamage(int sanityDamage)
    {
        curSanity -= sanityDamage;
        curSanity = Mathf.Clamp(curSanity, 0, maxSanity);
        GaugeUI.UpdateGauge(curSanity);
        Debug.Log("Sanity: " + curSanity);
    }
}
