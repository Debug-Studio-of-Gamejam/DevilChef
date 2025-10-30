// GlobalSettingsManager.cs
// 全局设置管理器，放在Persistent场景中，所有场景都可以使用

using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSettingsManager : MonoBehaviour
{
    public static GlobalSettingsManager Instance;

    private void Awake()
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

    /// <summary>
    /// 全局打开设置场景的方法
    /// 所有场景的设置按钮都可以调用这个方法
    /// </summary>
    public static void OpenSettingsScene()
    {
        Debug.Log("全局设置管理器：打开设置场景");

        // 播放按钮音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // 使用TransitionManager跳转到设置场景
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.Transition(TransitionManager.Instance.CurrentSceneName, "SettingScene");
        }
        else
        {
            Debug.LogError("TransitionManager.Instance 为 null！无法打开设置场景");
            
            // 备用方案：直接使用SceneManager
            SceneManager.LoadScene("SettingScene");
        }
    }

    /// <summary>
    /// 返回主界面（StartScene）
    /// </summary>
    public static void ReturnToMainMenu()
    {
        Debug.Log("全局设置管理器：返回主界面");

        // 播放按钮音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // 直接跳转到开始场景
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.Transition("SettingScene", "StartScene");
        }
        else
        {
            Debug.LogError("TransitionManager.Instance 为 null！无法返回主界面");
            
            // 备用方案：直接使用SceneManager
            SceneManager.LoadScene("StartScene");
        }
    }

    /// <summary>
    /// 返回之前的界面
    /// </summary>
    public static void ReturnToPreviousScene()
    {
        Debug.Log("全局设置管理器：返回之前的界面");

        // 播放按钮音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        // 使用TransitionManager返回上一个场景
        if (TransitionManager.Instance != null)
        {
            // 这里需要记录之前场景的逻辑，暂时先返回开始场景
            TransitionManager.Instance.ReturnToStartScene();
        }
        else
        {
            Debug.LogError("TransitionManager.Instance 为 null！无法返回之前的界面");
            
            // 备用方案：返回开始场景
            ReturnToMainMenu();
        }
    }
}