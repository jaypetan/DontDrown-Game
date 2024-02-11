using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextOnCollision : MonoBehaviour
{
    public GameObject Text;
    
    void Start()
    {
        Text.SetActive(false);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Text.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Text.SetActive(false);
        }
    }
}
