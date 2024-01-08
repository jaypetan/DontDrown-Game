using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestrictionBlocks : MonoBehaviour
{
    [SerializeField]
    private Text message;

    void Start()
    {
        message.enabled = false; 
    }

   void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            message.enabled = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            message.enabled = false;
        }
    }
}
