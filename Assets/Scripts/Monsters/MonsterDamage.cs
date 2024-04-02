using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    public PlayerOxygen playerOxygen;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOxygen.TakeDamage(damage);
        }
    }

}
