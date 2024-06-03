using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerOxygen : MonoBehaviour
{
    public int maxOxygen = 100;
    private int curOxygen;
    public int oxygenDecreaseRate = 1;
    bool oxygenDecreaseZone = true;
    float elapsed = 0f;
    public GaugeUI GaugeUI;

    public SpriteRenderer playerSprite;
    private Color originalColor;
    public float colorChangeDuration = 0.5f;

    public GameObject deathScreen;

    // Start is called before the first frame update
    public void Start()
    {
        // Store the original color
        originalColor = playerSprite.color;

        curOxygen = maxOxygen;
        SetHealth();
    }

    void SetHealth()
    {
        GaugeUI.SetInitialGauge(maxOxygen);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed >= 1f && oxygenDecreaseZone)
        {
            elapsed = elapsed % 1f;
            oxygenDecrease();
        }
    }

    private void oxygenDecrease()
    {
        curOxygen -= oxygenDecreaseRate;
        GaugeUI.UpdateGauge(curOxygen);

        // Debug.Log("Oxy: " + curOxygen);

        if (curOxygen <= 0)
        {
            if (deathScreen != null)
            {
                deathScreen.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        curOxygen -= damage;
        GaugeUI.UpdateGauge(curOxygen);
        StartCoroutine(ChangeColorTemporarily(Color.red));
        Debug.Log("Health: " + curOxygen);

        if (curOxygen <= 0)
        {
            if (deathScreen != null)
            {
                deathScreen.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColorTemporarily(Color color)
    {
        // Change to the new color
        playerSprite.color = color;

        // Wait for the specified duration
        yield return new WaitForSeconds(colorChangeDuration);

        // Revert back to the original color
        playerSprite.color = originalColor;
    }
}
