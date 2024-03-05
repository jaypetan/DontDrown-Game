using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    public GameObject player;
    public GameObject CutScene;

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
        Time.timeScale = 1;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
