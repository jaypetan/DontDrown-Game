using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider musicSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.Instance.musicVolume;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

}

