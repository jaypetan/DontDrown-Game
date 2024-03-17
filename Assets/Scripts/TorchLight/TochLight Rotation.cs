using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class TouchLightRotation : MonoBehaviour
{
    private float maxZRot = -50f;
    private float minZRot = -130f;

    private void Start()
    {

    }

    private void Update()
    {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        transform.up = direction;

        LimitRot();

    }

    private void LimitRot()
    {
        Vector3 playerEulerAngles = transform.localRotation.eulerAngles;

        playerEulerAngles.z = (playerEulerAngles.z > 180) ? playerEulerAngles.z -360 : playerEulerAngles.z;
        playerEulerAngles.z = Mathf.Clamp(playerEulerAngles.z, minZRot, maxZRot);

        transform.localRotation = Quaternion.Euler(playerEulerAngles);
    }
    
}
