using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyStun : MonoBehaviour
{
    public float stunDuration = 4f;
    private bool isPlayerStunned = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlayerStunned)
        {
            StunPlayer(collision.gameObject);
        }
    }

    private void StunPlayer(GameObject player)
    {
        // Add any additional logic you need when the player gets stunned
        Debug.Log("Player Stunned!");

        // Disable player controls or apply other effects
        
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.DisableMovement();
        }

        isPlayerStunned = true;

        // Start a coroutine to wait for the stun duration and then reset
        StartCoroutine(ResetStun(playerController));
    }
    IEnumerator ResetStun(PlayerController playerController)
    {
        yield return new WaitForSeconds(stunDuration);

        // Reset player state after stun duration
        // For example, re-enable controls
        if (playerController != null)
        {
            playerController.EnableMovement();
        }

        isPlayerStunned = false;
        Debug.Log("Player Stun Ended!");
    }
}
