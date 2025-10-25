
using UnityEngine;
using UnityEngine.UI; // 如果需要操作UI组件的话

public class StartSceneManager : MonoBehaviour
{
    // 如果需要引用场景中的特定UI元素，可以在这里声明
    [Header("UI References")]
    public Button startButton;
    public Button continueButton;
    public Button settingsButton;
    public Button quitButton; 

    void Start()
    {
        // 初始化按钮事件（如果不在编辑器中绑定）
        InitializeButtons();

        // 确保游戏时间正常（从暂停状态恢复）
        Time.timeScale = 1f;

        // 播放开始场景的背景音乐
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);
        }
    }

    void InitializeButtons()
    {
        // 如果通过编辑器拖拽绑定了按钮，这里可以留空

    }

    public void OnStartGameClick()
    {
        Debug.Log("开始游戏按钮被点击！");

        // 播放按钮音效
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // 调用GameManager开始新游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance 为 null！");
        }
    }

    public void OnContinueGameClick()
    {
        Debug.Log("继续游戏按钮被点击！");

        // 播放按钮音效
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // 调用GameManager继续游戏
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance 为 null！");
        }
    }

    public void OnSettingsClick()
    {
        Debug.Log("设置按钮被点击！");

        // 播放按钮音效
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // 打开设置面板
        if (UIManager.Instance != null)
        {
            UISettingsManager.Instance.OpenSettings();
        }
        else
        {
            Debug.LogError("UIManager.Instance 为 null！");
        }
    }

    public void OnQuitGameClick()
    {
        Debug.Log("退出游戏按钮被点击！");

        // 播放按钮音效
        if (AudioManager.Instance != null && AudioManager.Instance.buttonClickSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClickSFX);
        }

        // 退出游戏
#if UNITY_EDITOR
            // 在编辑器中停止播放
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在打包后的游戏中退出
        Application.Quit();
#endif
    }

    void Update()
    {
        // 可以在这里添加键盘快捷键支持
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnQuitGameClick();
        }
    }
}
