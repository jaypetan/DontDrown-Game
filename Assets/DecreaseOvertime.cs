using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecreaseOvertime : MonoBehaviour
{
    public Slider oxygenLevel;
    public float depletionRate = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        oxygenLevel.value = oxygenLevel.maxValue;        
    }

    // Update is called once per frame
    void Update()
    {
        DepleteOvertime();
    }

    void DepleteOvertime()
    {
        // Calculate new value based on depletion rate
        float newValue = oxygenLevel.value - depletionRate * Time.deltaTime;

        // Update the slider value
        oxygenLevel.value = newValue;

        //check if gauge is empty
        if (newValue == oxygenLevel.minValue)
        {
            Debug.Log("Gauge is Empty");
        }
    }
}
