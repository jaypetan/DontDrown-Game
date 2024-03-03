using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public GameObject player;
    public GameObject CutScene;
    public Transform nextSceneTarget;
    private bool CutSceneOn = false;

    void Update()
    {
       // Check if cutscene camera is on
        if (CutSceneOn == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndCutscene();
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
        TeleportPlayer(nextSceneTarget);
        CutScene.SetActive(true);
        // Disable player control
        StartCoroutine(TimePause());
        

    }
    IEnumerator TimePause()
    {
        yield return new WaitForSeconds(1f);
        CutSceneOn = true;
        Time.timeScale = 0;
    }

    void EndCutscene()
    {
        TeleportPlayer(nextSceneTarget);
        CutScene.SetActive(false);
        CutSceneOn = false;
        Time.timeScale = 1;
    }

    void TeleportPlayer(Transform TeleportPosition)
    {
        player.transform.position = TeleportPosition.position;
    }
}
