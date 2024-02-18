using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger!");
        // Enemies to take damage
        if (collider.gameObject.TryGetComponent<MonsterHealth>(out MonsterHealth monsterhealthComponent))
        {
            monsterhealthComponent.TakeDamage(5);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}
