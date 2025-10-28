// UIManager.cs

using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("UI面板")]
    public GameObject settingPanel;
    public GameObject inventoryPanel; // 背包面板
    public GameObject fadePanel;
    
    [Header("背包按钮")]
    public Button backpackButton; // 背包打开按钮
    
    private InventoryPanel inventoryPanelScript;
    
    private void Start()
    {
        fadePanel.SetActive(true);
        
        // 获取背包面板脚本
        if (inventoryPanel != null)
        {
            inventoryPanelScript = inventoryPanel.GetComponent<InventoryPanel>();
        }
        
        // 绑定背包按钮事件
        if (backpackButton != null)
        {
            backpackButton.onClick.AddListener(ToggleBackpack);
        }
    }

    private void Update()
    {
        // 快捷键支持：按B键打开背包
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBackpack();
        }
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
        
        // 调用背包面板的相应方法
        if (inventoryPanelScript != null)
        {
            if (show)
                inventoryPanelScript.OnInventoryOpened();
            else
                inventoryPanelScript.OnInventoryClosed();
        }
        
        // 当背包打开时，可以暂停游戏
        GameManager.Instance.isPaused = show;
        Time.timeScale = show ? 0 : 1; // 0代表暂停，1代表正常速度
    }

    /// <summary>
    /// 切换背包显示（按钮点击和快捷键）
    /// </summary>
    public void ToggleBackpack()
    {
        bool isInventoryOpen = inventoryPanel != null && inventoryPanel.activeSelf;
        ToggleInventory(!isInventoryOpen);
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