using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class TouchLightRotation : MonoBehaviour
{
    private float maxZRot = -50f;
    private float minZRot = -130f;

    private GameObject player;
    private PlayerController playerController;
    bool facingRight;

    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Read the movement value
        facingRight = playerController.facingRight;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 directionToMouse = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        if (facingRight == true)
        {
            // Moving right - ensure sprite faces right (assuming default sprite faces right)
            transform.up = directionToMouse.normalized;
        }
        else
        {
            // Moving left - flip sprite to face left
            transform.up = -directionToMouse.normalized;
        }

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
