using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject cutScene1;
    public GameObject cutScene2;

    public GameObject optionUI;
    public GameObject optionButton;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (cutScene2_active)
            {
                SceneManager.LoadSceneAsync(1);
                cutScene2_active = false;
            }

            if (cutScene1_active)
            {
                cutScene2.SetActive(true);
                cutScene2_active = true;
                cutScene1_active = false;
            }
        }
    }

    public void PlayGame()
    {
        cutScene1.SetActive(true);
        cutScene1_active = true;
    }
   
    public void Option()
    {
        optionUI.SetActive(true);
        optionButton.SetActive(false);
    }

    public void OptionOut()
    {
        optionUI.SetActive(false);
        optionButton.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();

        // if running in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif  
    }


}
