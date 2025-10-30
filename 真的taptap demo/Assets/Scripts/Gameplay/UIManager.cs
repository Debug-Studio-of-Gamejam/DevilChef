// UIManager.cs

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 在Unity编辑器里把这些UI面板拖拽赋值！ // 包括toolbar
    public GameObject settingPanel;
    public GameObject inventoryPanel; // 背包面板
    // ... 其他全局UI面板
    public GameObject fadePanel;
    
    [Header("全局UI按钮")]
    public Button globalBackpackButton; // 全局背包按钮（在所有场景中都显示）
    
    private void Start()
    {
        fadePanel.SetActive(true);
        
        // 初始化全局背包按钮
        InitializeGlobalBackpackButton();
    }
    
    /// <summary>
    /// 初始化全局背包按钮
    /// </summary>
    private void InitializeGlobalBackpackButton()
    {
        if (globalBackpackButton != null)
        {
            globalBackpackButton.onClick.AddListener(ToggleBackpack);
            Debug.Log("全局背包按钮初始化成功");
        }
        else
        {
            Debug.LogWarning("全局背包按钮未找到，请检查UI设置");
        }
    }
    
    /// <summary>
    /// 切换背包显示/隐藏（供全局按钮调用）
    /// </summary>
    public void ToggleBackpack()
    {
        if (inventoryPanel != null)
        {
            bool show = !inventoryPanel.activeSelf;
            ToggleInventory(show);
            
            // 通知背包面板状态变化
            var inventoryScript = inventoryPanel.GetComponent<InventoryPanel>();
            if (inventoryScript != null)
            {
                if (show)
                    inventoryScript.OnInventoryOpened();
                else
                    inventoryScript.OnInventoryClosed();
            }
            
            Debug.Log($"背包{(show ? "打开" : "关闭")}");
        }
    }

    // 显示/隐藏设置面板
    public void ToggleSettingPanel(bool show)
    {
        if (settingPanel != null)
        {
            settingPanel.SetActive(show);
        }
        else
        {
            Debug.LogWarning("settingPanel未赋值，请检查UIManager组件中的引用");
        }
    }

    // 显示/隐藏背包
    public void ToggleInventory(bool show)
    {
        inventoryPanel.SetActive(show);
        // 当背包打开时，可以暂停游戏
        GameManager.Instance.isPaused = show;
        Time.timeScale = show ? 0 : 1; // 0代表暂停，1代表正常速度
    }

    // public void TogglePanelBySceneName(string newSceneName)
    // {
    //     
    // }
    //
    // private void OnEnable()
    // {
    //     EventHandler.AfterSceneLoadEvent += TogglePanelBySceneName;
    // }
    //
    // private void OnDisable()
    // {
    //     EventHandler.AfterSceneLoadEvent -= TogglePanelBySceneName;
    // }
}