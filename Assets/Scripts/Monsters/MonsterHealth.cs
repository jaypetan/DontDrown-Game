using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 100f;
    [SerializeField] FloatingHealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
