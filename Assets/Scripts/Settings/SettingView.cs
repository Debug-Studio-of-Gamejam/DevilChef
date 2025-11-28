using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    public GameObject settingPanel;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    
    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.MusicVolumeKey, 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.SFXVolumeKey, 1f);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void SetSfxVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioName.按键音效);
        settingPanel.SetActive(false);
        TransitionManager.Instance.ReturnToStartScene();
    }

    public void CloseSettingPanel()
    {
        AudioManager.Instance.PlaySFX(AudioName.按键音效);
        GameManager.Instance.isPaused = false;
        settingPanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        AudioManager.Instance.PlaySFX(AudioName.按键音效);
        GameManager.Instance.isPaused = true;
        settingPanel.SetActive(true);
    }
}
