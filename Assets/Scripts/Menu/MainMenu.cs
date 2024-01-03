using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider[] mMusicSliders;
    public Slider mSFXSlider;

    public void loadScene(int iSceneIndex)
    {
        SceneManager.LoadScene(iSceneIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void toggleMusic(int iSourceIndex)
    {
        AudioManager.sInstance.toggleMusic(iSourceIndex);
    }

    public void toggleSFX()
    {
        AudioManager.sInstance.toggleSFX();
    }

    public void setMusicVolume(int iSourceIndex)
    {
        AudioManager.sInstance.setMusicVolume(mMusicSliders[iSourceIndex].value, iSourceIndex);
    }

    public void setSFXVolume()
    {
        AudioManager.sInstance.setSFXVolume(mSFXSlider.value);
    }
}
