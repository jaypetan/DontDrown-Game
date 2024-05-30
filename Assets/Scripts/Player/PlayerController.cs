using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float turnSpeed = 10f;
    public float acceleration = 1000f;
    public float speedMultiplier = 2f;
    public float speedBoostCooldown = 2f;
    public float speedBoostDuration = 2f;

    public bool isSpeedBoostActive = false;
    public bool isSpeedBoostOnCooldown = false;
    private float speedBoostCooldownTimer = 0f;

    private Rigidbody2D rb;
    public Vector2 moveInput;
    public Vector2 currentVelocity;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction boostAction;
    public PlayerSanity moveSanity;

    public bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        // Assuming "PlayerControls" is the name of your action map
        var actionMap = playerInput.actions.FindActionMap("PlayerControls");
        moveAction = actionMap.FindAction("Move");
        boostAction = actionMap.FindAction("SpeedBoost");
    }

    public void EnableMovement()
    {
        moveAction.Enable();
    }

    public void DisableMovement()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        // Handle cooldown timer in Update to ensure it decreases over time
        if (isSpeedBoostOnCooldown)
        {
            speedBoostCooldownTimer -= Time.deltaTime;
            if (speedBoostCooldownTimer <= 0)
            {
                isSpeedBoostOnCooldown = false;
                speedBoostCooldownTimer = 0; // Ensure the timer doesn't go negative
            }
        }

        if (moveSanity.curSanity <= 80 && moveSanity.curSanity >= 61)
        {
            moveSpeed = 8f;
        } else if (moveSanity.curSanity <= 60 && moveSanity.curSanity >= 41) 
        {
            moveSpeed = 10f;
        } else if (moveSanity.curSanity <= 40 && moveSanity.curSanity >= 21) 
        {
            moveSpeed = 12f;
        } else if (moveSanity.curSanity <= 20) 
        {
            moveSpeed = 14f;
        } else {
            moveSpeed = 6f;
        }
    }

    private void FixedUpdate()
    {
        // Read the movement value
        moveInput = moveAction.ReadValue<Vector2>();
        MoveSubmarine();
    }

    private Vector2 smoothDampVelocity;
    private void MoveSubmarine()
    {
        float currentSpeed = isSpeedBoostActive ? moveSpeed * speedMultiplier : moveSpeed;

        if (boostAction.ReadValue<float>() > 0 && !isSpeedBoostActive && !isSpeedBoostOnCooldown)
        {
            StartCoroutine(ActivateSpeedBoost(speedBoostDuration));
        }

        Vector2 targetVelocity = moveInput * currentSpeed;
        currentVelocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref smoothDampVelocity, 0.3f);
        rb.velocity = currentVelocity;

        if (moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, turnSpeed * Time.fixedDeltaTime);
        }

        if (moveInput.x > 0)
        {
            // Moving right - ensure sprite faces right (assuming default sprite faces right)
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
            facingRight = true;
        }
        else if (moveInput.x < 0)
        {
            // Moving left - flip sprite to face left
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), -Mathf.Abs(transform.localScale.y), transform.localScale.z);
            facingRight = false;
        }
    }

    IEnumerator ActivateSpeedBoost(float duration)
    {
        isSpeedBoostActive = true;
        yield return new WaitForSeconds(duration);
        isSpeedBoostActive = false;

        // Start cooldown after boost ends
        isSpeedBoostOnCooldown = true;
        speedBoostCooldownTimer = speedBoostCooldown;
    }

    public IEnumerator StunPlayer(float stunDuration)
    {
        DisableMovement(); // Immediately disable movement
        yield return new WaitForSeconds(stunDuration); // Wait for the stun duration
        EnableMovement(); // Re-enable movement after the duration
    }
}
