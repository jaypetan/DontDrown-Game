using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLightRotation : MonoBehaviour
{
    public GameObject player;

    private SpriteRotate sprite;

    private void Start()
    {
        sprite = player.GetComponent<SpriteRotate>();
    }

    private void FixedUpdate()
    {
        // Rotate angle of light
        Vector3 difference;

        if (sprite.facingRight == true)
        {
            Debug.Log("Face Right");
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }
        else
        {
            Debug.Log("Face Left");
            difference = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Clamp the rotationZ to the specified range (-40 to 40 degrees)
        rotationZ = Mathf.Clamp(rotationZ, -40f, 40f);

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        

        // Testing some rotation code
        // if (rotationZ < -40 || rotationZ > 40)
        // {

        //     if (player.transform.eulerAngles.y == 0)
        //     {

        //         transform.localRotation = Quaternion.Euler(180,0,-rotationZ);

        //     } else if (player.transform.eulerAngles.y == 180) 
        //     {

        //         transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);

        //     }
        // }
    }
}
