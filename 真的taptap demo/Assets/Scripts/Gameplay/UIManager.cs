// UIManager.cs
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    // 在Unity编辑器里把这些UI面板拖拽赋值！
    public GameObject startMenuPanel;
    public GameObject settingPanel;
    public GameObject inventoryPanel; // 背包面板
    // ... 其他全局UI面板

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // UIManager通常也在开始场景创建，但可以根据需要决定是否DontDestroyOnLoad
            // 如果每个场景的UI都不同，那就不需要DontDestroyOnLoad，而是在每个场景重置引用。
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 显示/隐藏开始菜单
    public void ToggleStartMenu(bool show)
    {
        startMenuPanel.SetActive(show);
    }

    // 显示/隐藏设置面板
    public void ToggleSettingPanel(bool show)
    {
        settingPanel.SetActive(show);
    }

    // 显示/隐藏背包
    public void ToggleInventory(bool show)
    {
        inventoryPanel.SetActive(show);
        // 当背包打开时，可以暂停游戏
        GameManager.Instance.isPaused = show;
        Time.timeScale = show ? 0 : 1; // 0代表暂停，1代表正常速度
    }
}