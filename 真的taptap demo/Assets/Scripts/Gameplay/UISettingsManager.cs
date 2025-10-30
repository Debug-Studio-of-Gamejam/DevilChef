// Scripts/Managers/UISettingsManager.cs
using UnityEngine;
using UnityEngine.UI;

public class UISettingsManager : MonoBehaviour
{
    public static UISettingsManager Instance;

    [Header("Settings Panel")]
    public GameObject settingsPanel;

    [Header("全屏设置")]
    public Toggle fullscreenToggle;

    [Header("音量设置")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("按钮引用")]
    public Button closeButton;
    public Button applyButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 确保面板初始隐藏
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
        // 绑定UI事件
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

    #region 弹窗控制
    public void ToggleSettingsPanel()
    {
        if (settingsPanel == null) return;

        bool isActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(isActive);

        // 更新设置显示
        if (isActive)
        {
            LoadSettings();
        }

        // 暂停游戏逻辑
        UpdateTimeScale(isActive);

        // 播放音效
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















    /// <summary>
    /// 返回主界面（StartScene）
    /// </summary>
    public void ReturnToStartScene()
    {
        Debug.Log("UISettingsManager：返回主界面");

        // 播放按钮音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // 关闭设置面板
        CloseSettings();

        // 使用TransitionManager返回开始场景
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.ReturnToStartScene();
        }
        else
        {
            Debug.LogError("TransitionManager.Instance 为 null！无法返回主界面");
            
            // 备用方案：直接使用SceneManager
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        }
    }

    /// <summary>
    /// 记录进入设置场景前的场景名
    /// </summary>
    private string previousSceneName = "";
    
    /// <summary>
    /// 设置进入设置场景前的场景名
    /// </summary>
    public void SetPreviousScene(string sceneName)
    {
        previousSceneName = sceneName;
        Debug.Log($"记录进入设置前的场景: {previousSceneName}");
    }
    
    /// <summary>
    /// 返回之前的界面
    /// </summary>
    public void ReturnToPreviousScene()
    {
        Debug.Log($"UISettingsManager：返回之前的界面，之前场景: {previousSceneName}");

        // 播放按钮音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // 关闭设置面板
        CloseSettings();

        // 使用TransitionManager返回上一个场景
        if (TransitionManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(previousSceneName) && previousSceneName != "SettingScene")
            {
                // 返回之前记录的场景
                TransitionManager.Instance.Transition("SettingScene", previousSceneName);
            }
            else
            {
                // 如果没有记录或记录无效，返回开始场景
                Debug.LogWarning("没有有效的之前场景记录，返回开始场景");
                TransitionManager.Instance.ReturnToStartScene();
            }
        }
        else
        {
            Debug.LogError("TransitionManager.Instance 为 null！无法返回之前的界面");
            
            // 备用方案：返回开始场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
        }
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

    #region 设置功能
    public void OnFullscreenToggleChanged(bool isOn)
    {
        Screen.fullScreen = isOn;
        Debug.Log($"全屏模式: {(isOn ? "开启" : "关闭")}");
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

    #region 设置持久化
    public void SaveSettings()
    {
        // 全屏设置
        if (fullscreenToggle != null)
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);

        // 音量设置
        if (masterVolumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

        if (musicVolumeSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);

        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

        PlayerPrefs.Save();
        Debug.Log("设置已保存");
    }

    public void LoadSettings()
    {
        // 全屏设置
        if (fullscreenToggle != null)
        {
            bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            fullscreenToggle.isOn = fullscreen;
            Screen.fullScreen = fullscreen;
        }

        // 音量设置
        if (masterVolumeSlider != null)
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        Debug.Log("设置已加载");
    }
    #endregion

    void OnDestroy()
    {
        // 清理事件绑定
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