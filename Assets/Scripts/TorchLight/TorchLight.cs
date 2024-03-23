using UnityEngine;

public class Torchlight : MonoBehaviour
{
    public float torchDmg = 10f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Damaged");
            other.GetComponent<Enemy>().TakeDamage(torchDmg); 
        }
    }
}
