using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public GameObject cutsceneCamera;
    public GameObject mainGameCamera;
    public GameObject nextGameCamera;
    public Transform teleportTarget;

    [SerializeField]
    private Text skipCutscene;

    private void Start()
    {
        skipCutscene.enabled = false;
    }
    void Update()
    {
       // Check if cutscene camera is on
        if (cutsceneCamera != null && cutsceneCamera.activeSelf )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndCutscene();
                skipCutscene.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCutscene();
        }    
    }

    void StartCutscene()
    {
        // Disable player control
        player.GetComponent<Rigidbody2D>().isKinematic = true;

        // Activate cutscene camera
        cutsceneCamera.SetActive(true);
        mainGameCamera.SetActive(false);

        skipCutscene.enabled = true;

    }

    void EndCutscene()
    {
        TeleportPlayer();

        player.GetComponent<Rigidbody2D>().isKinematic = false;

        cutsceneCamera.SetActive(false); 
        nextGameCamera.SetActive(true);
    }

    void TeleportPlayer()
    {
        player.transform.position = teleportTarget.position;
    }
}
