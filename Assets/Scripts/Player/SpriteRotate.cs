using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool facingRight = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detect player input to flip the character
        float moveInput = Input.GetAxis("Horizontal");

        // If the input indicates movement to the left and the character is facing right, or vice versa
        if ((moveInput < 0 && facingRight) || (moveInput > 0 && !facingRight))
        {
            // Flip the character horizontally
            Flip();
        }
    }

    void Flip()
    {
        // Switch the direction the character is facing
        facingRight = !facingRight;

        // Get the current scale
        Vector3 scale = transform.localScale;

        // Flip the scale along the X-axis
        scale.x *= -1;
        transform.localScale = scale;
    }
}
