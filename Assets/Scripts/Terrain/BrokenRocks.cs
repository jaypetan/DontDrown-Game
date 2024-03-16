using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenRocks : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if (collision.gameObject.tag == "Player" && player.isSpeedBoostOnCooldown == true)
            {
                animator.SetBool("IsBroken", true);
                StartCoroutine(BreakWall());
            }
        }
    }

    IEnumerator BreakWall()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<EdgeCollider2D>().enabled = false;
    }
}
