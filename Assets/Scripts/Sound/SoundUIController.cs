using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    public Slider _musicSlider, _sfxSlider;
    public GameObject soundCanvas;

    private bool isCanvasActive = false;
    SoundManager soundManager;

    private void Start()
    {
        if (soundManager == null)
        {
            soundManager = SoundManager.instance;
            if (soundManager == null)
            {
                Debug.LogError("Sound Manager instance not found!");
                return;
            }
        }
    }

    public void ToggleMusic()
    {
        soundManager.ToggleMusic();
    }

    public void ToggleSFX()
    {
        soundManager.ToggleSFX();
    }

    public void MusicVolume()
    {
        soundManager.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
        soundManager.SFXVolume(_sfxSlider.value);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCanvasActive = !isCanvasActive;
            soundCanvas.SetActive(isCanvasActive);
        }

    }

}
