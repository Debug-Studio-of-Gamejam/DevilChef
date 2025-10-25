// AudioManager.cs
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip buttonClickSFX;
    public AudioClip mainMenuMusic;

    // 音量设置
    private float _masterVolume = 1f;
    private float _musicVolume = 1f;
    private float _sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadAudioSettings();
    }

    // 音量控制方法
    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        UpdateAudioVolumes();
        PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
    }

    private void UpdateAudioVolumes()
    {
        if (musicSource != null)
            musicSource.volume = _masterVolume * _musicVolume;

        if (sfxSource != null)
            sfxSource.volume = _masterVolume * _sfxVolume;
    } 

    private void LoadAudioSettings()
    {
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        UpdateAudioVolumes();
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxSource != null && sfxClip != null)
        {
            sfxSource.PlayOneShot(sfxClip);
        }
    }

    // 现有的音频播放方法
    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSFX != null)
            sfxSource.PlayOneShot(buttonClickSFX);
    }
}