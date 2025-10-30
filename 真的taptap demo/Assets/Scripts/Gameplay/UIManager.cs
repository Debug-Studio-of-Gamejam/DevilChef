// UIManager.cs

using System;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // 在Unity编辑器里把这些UI面板拖拽赋值！ // 包括toolbar
    public GameObject settingPanel;
    public GameObject inventoryPanel; // 背包面板
    // ... 其他全局UI面板
    public GameObject fadePanel;
    
    private void Start()
    {
        //fadePanel.SetActive(true);
    }

    // 显示/隐藏设置面板
    public void ShowSettingPanel()
    {
        Debug.Log("ShowSettingPanel");
        settingPanel.SetActive(true);
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