using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
