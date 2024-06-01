using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkMonsterHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider; // Reference to the UI Slider
    public bool monsterDied = false;

    public GameObject GoToEndScene;
    public GameObject CheckPoint;
    public GameObject monsterHealthBar;

    public GameObject DialogueBox;
    public Dialogue dialogue;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        // Handle the monster's death (e.g., play an animation, disable the GameObject, etc.)
        Destroy(gameObject);
        monsterDied = true;

        monsterHealthBar.SetActive(false);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

        SharkMonsterController sharkController = GetComponent<SharkMonsterController>();
        if (sharkController != null)
        {
            sharkController.DestroyAllSegments();
        }

        EndGame();
    }

    void EndGame()
    {
        GoToEndScene.SetActive(true);
        if(CheckPoint != null)
        {
            CheckPoint.SetActive(false);
        }
    }
}
