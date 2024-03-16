using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    [SerializeField] public Slider gaugeLevel;

    // Start is called before the first frame update
    void Start()
    {       
    }

    // Update is called once per frame
    public void SetInitialGauge(int maxAmount)
    {
        gaugeLevel.maxValue = maxAmount;
        gaugeLevel.value = maxAmount;
    }

    public void UpdateGauge(int currentValue)
    {
        gaugeLevel.value = currentValue;
        // Debug.Log("Updated GaugeLevel" + gaugeLevel.value);
    }
}
