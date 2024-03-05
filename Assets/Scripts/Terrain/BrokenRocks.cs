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
        SpeedBoost speed = collision.gameObject.GetComponent<SpeedBoost>();
        if (speed != null)
        {
            if (collision.gameObject.tag == "Player" && speed.isOnCooldown == true)
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
