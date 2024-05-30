using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityCollision : MonoBehaviour
{
    public int sanityDamage = 4;
    public PlayerSanity sanityCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            sanityCollision.TakeSanityDamage(sanityDamage);
        }
    }
}
