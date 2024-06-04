using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;

    void Start()
    {
        AudioManager.Instance.PlayMusic(backgroundMusic);
    }
}

