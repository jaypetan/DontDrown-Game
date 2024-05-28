using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public PlayerOxygen player;
    public string levelOne = "Level 1";
    public string levelTwo = "Level 2";
    public string levelThree = "Level 3";
    public string levelFour = "Level 4";
    

    void Start()
    {
        int currentLevel = GetCurrentLevel();
        player.maxOxygen = GetMaxHPForLevel(currentLevel);
        player.Start(); // Reinitialize the player with new maxHP
    }

    int GetCurrentLevel()
    {
        // Your logic to get current level
        if(SceneManager.GetActiveScene().name == levelOne)
        {
            return 1;
        } else if(SceneManager.GetActiveScene().name == levelTwo)
        {
            return 2;
        }else if(SceneManager.GetActiveScene().name == levelThree)
        {
            return 3;
        } else if(SceneManager.GetActiveScene().name == levelFour)
        {
            return 3;
        } else
        {
            return 1;
        }
    }

    int GetMaxHPForLevel(int level)
    {
        // Your logic to get maxHP for a given level
        switch (level)
        {
            case 1: return 100;
            case 2: return 150;
            case 3: return 200;
            default: return 100;
        }
    }

}
