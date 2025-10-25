// Scripts/Managers/UISettingsManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UISettingsManager : MonoBehaviour
{
    public static UISettingsManager Instance;

    [Header("Settings Panel")]
    public GameObject settingsPanel;

    [Header("ȫ������")]
    public Toggle fullscreenToggle;

    [Header("��������")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("��ť����")]
    public Button closeButton;
    public Button applyButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ȷ������ʼ����
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeUIComponents();
        LoadSettings();
    }

    private void InitializeUIComponents()
    {
        // ��UI�¼�
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);

        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettings);

        if (applyButton != null)
            applyButton.onClick.AddListener(ApplySettings);
    }

    #region ��������
    public void ToggleSettingsPanel()
    {
        if (settingsPanel == null) return;

        bool isActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(isActive);

        // ����������ʾ
        if (isActive)
        {
            LoadSettings();
        }

        // ��ͣ��Ϸ�߼�
        UpdateTimeScale(isActive);

        // ������Ч
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }

    public void OpenSettings()
    {
        if (settingsPanel != null && !settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(true);
            LoadSettings();
            UpdateTimeScale(true);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButtonClick();
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
            UpdateTimeScale(false);
            SaveSettings();

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayButtonClick();
        }
    }

    public void ApplySettings()
    {
        SaveSettings();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
    }

    private void UpdateTimeScale(bool isPaused)
    {
        if (IsInGameScene())
        {
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }

    private bool IsInGameScene()
    {
        if (GameManager.Instance == null) return false;

        string currentScene = GameManager.Instance.currentSceneName;
        return currentScene != "StartScene" && currentScene != "OpeningScene";
    }
    #endregion

    #region ���ù���
    public void OnFullscreenToggleChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
        Debug.Log($"ȫ��ģʽ: {(isOn ? "����" : "�ر�")}");
    }

    public void OnMasterVolumeChanged(float volume)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMasterVolume(volume);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(volume);
    }
    #endregion

    #region ���ó־û�
    public void SaveSettings()
    {
        // ȫ������
        if (fullscreenToggle != null)
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        // ��������
        if (masterVolumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

        if (musicVolumeSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        PlayerPrefs.Save();
        Debug.Log("�����ѱ���");
    }

    public void LoadSettings()
    {
        // ȫ������
        if (fullscreenToggle != null)
        {
            bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = fullscreen;
            Screen.fullScreen = fullscreen;
        }

        // ��������
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        Debug.Log("�����Ѽ���");
    }
    #endregion

    void OnDestroy()
    {
        // �����¼���
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggleChanged);

        if (masterVolumeSlider != null)
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);

        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }
}