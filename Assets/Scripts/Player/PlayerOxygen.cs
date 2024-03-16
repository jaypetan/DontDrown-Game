using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public int maxOxygen = 100;
    private int curOxygen;
    public int oxygenDecreaseRate = 1;
    bool oxygenDecreaseZone = true;
    float elapsed = 0f;
    public GaugeUI GaugeUI;

    // Start is called before the first frame update
    void Start()
    {
        curOxygen = maxOxygen;
        SetHealth();
    }

    void SetHealth()
    {
        GaugeUI.SetInitialGauge(maxOxygen);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= 1f && oxygenDecreaseZone)
        {
            elapsed = elapsed % 1f;
            oxygenDecrease();
        }
    }

    private void oxygenDecrease()
    {
        curOxygen -= oxygenDecreaseRate;
        GaugeUI.UpdateGauge(curOxygen);

        // Debug.Log("Oxy: " + curOxygen);

        if (curOxygen <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        curOxygen -= damage;
        GaugeUI.UpdateGauge(curOxygen);
        Debug.Log("Health: " + curOxygen);

        if (curOxygen <= 0)
        {
            Destroy(gameObject);
        }
    }
}
