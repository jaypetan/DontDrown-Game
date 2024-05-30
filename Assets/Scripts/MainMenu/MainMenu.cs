using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject cutScene1;
    public GameObject cutScene2;

    private bool cutScene1_active = false;
    private bool cutScene2_active = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        cutScene1.SetActive(false);
        cutScene2.SetActive(false);
    }

    void Update()
    {
        if (cutScene1_active && Input.GetKeyDown(KeyCode.Space))
        {
            cutScene2.SetActive(true);
            cutScene2_active = true;
            cutScene1_active = false;
        } else if (cutScene2_active && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadSceneAsync(1);
            cutScene2_active = false;
        }
    }

    public void PlayGame()
    {
        cutScene1.SetActive(true);
        cutScene1_active = true;
    }
    public void nextScene()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
