using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class collid : MonoBehaviour
{
    [SerializeField]
    private Text checkpointMessage;

    void Start()
    {
        checkpointMessage.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            checkpointMessage.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkpointMessage.enabled = false;
        }
    }
}
