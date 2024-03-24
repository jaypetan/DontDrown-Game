using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoost : MonoBehaviour
{

    public KeyCode increaseSpeed = KeyCode.X;

    public Slider cooldownSlider;
    public float cooldownDuration = 4f;
    private float cooldownTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(increaseSpeed) && cooldownTimer <= 0f)
        {
            cooldownTimer = cooldownDuration;         // Start cooldown after press
        }

        // Update the cooldown timer
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);

        // Update Cooldown UI
        cooldownSlider.value = cooldownTimer;

    }
}
