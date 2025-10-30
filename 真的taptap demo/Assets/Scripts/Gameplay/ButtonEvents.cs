// ButtonEvents.cs
// 简单的按钮事件脚本，专门用于按钮绑定

using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel; // 直接在Inspector中拖拽赋值
    private bool isSceneLoaded = false; // 标记场景是否已加载完成
    
    private void OnEnable()
    {
        // 订阅场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        // 取消订阅场景加载事件
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 每次场景加载时都隐藏Panel
        if (scene.name == "SettingScene")
        {
            EnsurePanelHidden();
            isSceneLoaded = true;
        }
    }
    
    private void Start()
    {
        // 确保当前场景加载时Panel隐藏
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentScene == "SettingScene" && !isSceneLoaded)
        {
            EnsurePanelHidden();
            isSceneLoaded = true;
        }
    }
    
    private void EnsurePanelHidden()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            Debug.Log("CreditsPanel已设置为隐藏状态（场景加载时）");
        }
    }
    /// <summary>
    /// 打开设置场景 - 所有场景的设置按钮都绑定这个方法
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("按钮事件：打开设置场景");
        
        // 检查当前是否已经在设置场景中
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        if (currentScene == "SettingScene")
        {
            Debug.Log("已经在设置场景中，无需重复加载");
            return;
        }
        
        // 记录进入设置场景前的场景
        if (UISettingsManager.Instance != null)
        {
            UISettingsManager.Instance.SetPreviousScene(currentScene);
        }
        
        // 加载设置场景
        Debug.Log($"从 {currentScene} 加载设置场景");
        
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.Transition(currentScene, "SettingScene");
        }
        else
        {
            Debug.LogWarning("TransitionManager.Instance 为 null，直接加载设置场景");
            // 备用方案：直接使用SceneManager
            UnityEngine.SceneManagement.SceneManager.LoadScene("SettingScene");
        }
    }
    
    /// <summary>
    /// 返回主界面 - SettingScene中的"返回主页"按钮绑定这个方法
    /// </summary>
    public void ReturnToMainMenu()
    {
        Debug.Log("按钮事件：返回主界面");
        if (UISettingsManager.Instance != null)
        {
            UISettingsManager.Instance.ReturnToStartScene();
        }
        else
        {
            Debug.LogError("UISettingsManager.Instance 为 null！无法返回主界面");
            // 备用方案：使用全局设置管理器
            GlobalSettingsManager.ReturnToMainMenu();
        }
    }
    
    /// <summary>
    /// 返回之前的界面 - SettingScene中的"返回之前界面"按钮绑定这个方法
    /// </summary>
    public void ReturnToPrevious()
    {
        Debug.Log("按钮事件：返回之前界面");
        if (UISettingsManager.Instance != null)
        {
            UISettingsManager.Instance.ReturnToPreviousScene();
        }
        else
        {
            Debug.LogError("UISettingsManager.Instance 为 null！无法返回之前界面");
            // 备用方案：使用全局设置管理器
            GlobalSettingsManager.ReturnToPreviousScene();
        }
    }
    
    /// <summary>
    /// 关闭设置面板 - SettingScene中的关闭按钮绑定这个方法
    /// </summary>
    public void CloseSettings()
    {
        Debug.Log("按钮事件：关闭设置面板");
        if (UISettingsManager.Instance != null)
        {
            UISettingsManager.Instance.CloseSettings();
        }
        else
        {
            Debug.LogError("UISettingsManager.Instance 为 null！无法关闭设置面板");
        }
    }
    
    /// <summary>
    /// 保存游戏 - SettingScene中的存档按钮绑定这个方法
    /// </summary>
    public void SaveGame()
    {
        Debug.Log("按钮事件：保存游戏");
        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.SaveGame();
        }
        else
        {
            Debug.LogError("SaveLoadManager.Instance 为 null！无法保存游戏");
        }
    }
    
    /// <summary>
    /// 应用设置 - SettingScene中的应用按钮绑定这个方法
    /// </summary>
    public void ApplySettings()
    {
        Debug.Log("按钮事件：应用设置");
        if (UISettingsManager.Instance != null)
        {
            UISettingsManager.Instance.ApplySettings();
        }
        else
        {
            Debug.LogError("UISettingsManager.Instance 为 null！无法应用设置");
        }
    }

    /// <summary>
    /// 显示制作人员面板 - CreditsNameButton绑定这个方法
    /// </summary>
    public void ShowCredits()
    {
        Debug.Log("按钮事件：显示制作人员面板");
        
        if (creditsPanel != null)
        {
            // 直接显示Panel
            creditsPanel.SetActive(true);
            Debug.Log("制作人员面板已显示");
        }
        else
        {
            Debug.LogError("CreditsPanel引用为空！请在Inspector中拖拽赋值");
        }
    }

    /// <summary>
    /// 隐藏制作人员面板 - CreditsPanel中的BackButton绑定这个方法
    /// </summary>
    public void HideCredits()
    {
        Debug.Log("按钮事件：隐藏制作人员面板（通过返回按钮）");
        
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
            Debug.Log("制作人员面板已通过返回按钮隐藏");
        }
        else
        {
            Debug.LogError("CreditsPanel引用为空！请在Inspector中拖拽赋值");
        }
    }
    
    // 删除了所有复杂的查找方法，现在直接使用序列化字段
}