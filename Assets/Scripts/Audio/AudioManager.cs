using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager sInstance;

    public Sound[] mMusicSounds;
    public Sound[] mSfxSounds;
    public AudioSource[] mMusicSources;
    public AudioSource mSfxSource;

    private void Awake()
    {
        if(sInstance == null)
        {
            sInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        playMusic("Music01", 0);
        playMusic("CitySounds", 1);

        setMusicVolume(0.1f, 0);
        setMusicVolume(0.15f, 1);
        setSFXVolume(0.5f);
    }

    public void playMusic(string iName, int iSourceIndex)
    {
        Sound lSound = Array.Find(mMusicSounds, x => x.mName == iName); // Find sound using name

        if(lSound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            mMusicSources[iSourceIndex].clip = lSound.mClip;
            mMusicSources[iSourceIndex].Play();
        }
    }

    public void playSFX(string iName)
    {
        Sound lSound = Array.Find(mSfxSounds, x => x.mName == iName); // Find sound using name

        if(lSound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            mSfxSource.PlayOneShot(lSound.mClip);
        }
    }

    public void toggleMusic(int iSourceIndex)
    {
        mMusicSources[iSourceIndex].mute = !mMusicSources[iSourceIndex].mute;
    }

    public void toggleSFX()
    {
        mSfxSource.mute = !mSfxSource.mute;
    }

    public void setMusicVolume(float iVolume, int iSourceIndex)
    {
        mMusicSources[iSourceIndex].volume = iVolume;
    }

    public void setSFXVolume(float iVolume)
    {
        mSfxSource.volume = iVolume;
    }
}
