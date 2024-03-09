using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float controlSpeed = 5; 
    [SerializeField]
    private float rotationSpeed;
    public Rigidbody2D rb;
    // public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component 
 
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0f,0f,270f);
    }

    // Update is called once per frame
    void Update()
    {

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(h,v);
            float inputMagnitude = Mathf.Clamp01(move.magnitude);
            move.Normalize();
            
            transform.Translate(move * controlSpeed * inputMagnitude * Time.deltaTime, Space.World);

            if (move != Vector2.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, move);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        //     // Vector3 move = new Vector3(h, v, 0);
        //     // transform.position += move * controlSpeed * Time.deltaTime;
        //     // Vector3 direction = move.normalized;

        //     // Ensure the player's rotation is fixed
        //     rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //     // keep the original facing direction
        //     Vector3 currentDirection = transform.up;

        //     if (move != Vector3.zero)
        //     {
        //         transform.up = direction;
        //     }

        // // Set the facing direction back to the original direction 
        // transform.up = currentDirection;

    }
    public void DisableMovement()
    {
        controlSpeed = 0f;
    }

    public void EnableMovement()
    {
        controlSpeed = 5f;
    }

}
