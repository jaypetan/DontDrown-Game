using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        // Get input values for horizontal and vertical movement 
        float h = Input.GetAxis("Horizontal"); 
        float v = Input.GetAxis("Vertical"); 
 
        // Create a movement vector based on input 
        Vector3 move = new Vector3(h, v, 0); 
 
        // Move the player using transform.position 
        transform.position += move * controlSpeed * Time.deltaTime; 
 
        // Calculate the direction vector from the movement input 
        Vector3 direction = move.normalized; 
 
        // Rotate the player's sprite to face the movement direction 
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
        // If horizontalInput is 0, the sprite remains in its current state 
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
