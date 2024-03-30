using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sound[] musicSounds, sfxSounds, sfxSounds2, stepsSounds;
    public AudioSource musicSource, sfxSource, sfxSource2, stepsSource;

    public bool destroyOnNextLoad = false;

    #region Unity Methods

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (destroyOnNextLoad)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    #region General Methods

    //Set the flag to destroy this object on next scene load
    public void DestroyThisOnLoad()
    {
        destroyOnNextLoad = true;
    }

    #endregion

    #region Music Public Methods

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX2(string name)
    {
        Sound s = Array.Find(sfxSounds2, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource2.PlayOneShot(s.clip);
        }
    }

    public void PlaySteps(string name)
    {
        Sound s = Array.Find(stepsSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            stepsSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        sfxSource2.mute = !sfxSource.mute;
    }

    public void ToggleSFX()
    {        
        sfxSource.mute = !sfxSource.mute;        
        stepsSource.mute = !stepsSource.mute;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        stepsSource.volume = volume - 0.25f;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;        
    }
    

    #endregion


}
