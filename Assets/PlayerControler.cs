using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASD : MonoBehaviour
{
    public float controlSpeed = 5;

    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on GameObject: " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3( h, v, 0);
        transform.position += move * controlSpeed * Time.deltaTime;
        Vector3 direction = move.normalized;

        if (move != Vector3.zero)
        {
            transform.up = direction;
            
            // Flip the sprite horizontally based on movement direction
            FlipSprite(h);
        }

    }

    void FlipSprite(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            // Flip the sprite when moving left
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            // Unflip the sprite when moving right
            spriteRenderer.flipX = false;
        }
    }
}
