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
    
    float musicVolume;
    float sfxVolume;
    
    private const string SFXVolumeKey = "SFXVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private void Awake()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
        PlayerPrefs.Save();
    }

    public void ReturnToMainMenu()
    {
        settingPanel.SetActive(false);
        TransitionManager.Instance.ReturnToStartScene();
    }

    public void CloseSettingPanel()
    {
        GameManager.Instance.isPaused = false;
        settingPanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        GameManager.Instance.isPaused = true;
        settingPanel.SetActive(true);
    }
}
