using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextOnEnter : MonoBehaviour
{
    public GameObject Text;

    void Start()
    {
        Text.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the GameObject that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the GameObject that entered the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            Text.SetActive(false);
        }
    }
}
