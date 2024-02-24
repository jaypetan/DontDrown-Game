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

    public GameObject skipCutscene;

    private void Start()
    {
        skipCutscene.SetActive(false);
    }
    void Update()
    {
       // Check if cutscene camera is on
        if (cutsceneCamera != null && cutsceneCamera.activeSelf )
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndCutscene();
                skipCutscene.SetActive(false);
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
        Time.timeScale = 0f;

        // Activate cutscene camera
        cutsceneCamera.SetActive(true);
        mainGameCamera.SetActive(false);

        skipCutscene.SetActive(true);

    }

    void EndCutscene()
    {
        TeleportPlayer();
        Time.timeScale = 1f;

        cutsceneCamera.SetActive(false); 
        nextGameCamera.SetActive(true);
    }

    void TeleportPlayer()
    {
        player.transform.position = teleportTarget.position;
    }
}
