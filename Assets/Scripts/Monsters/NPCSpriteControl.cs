using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpriteControl : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Get the velocity of the Rigidbody2D
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Flip the sprite based on velocity
        if (velocity.x < -0.01f)
        {
            spriteRenderer.flipX = false; // Facing left
        }
        else if (velocity.x > 0.01f)
        {
            spriteRenderer.flipX = true; // Facing right
        }
    }
}

