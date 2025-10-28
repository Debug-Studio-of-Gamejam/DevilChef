using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

    [Header("玩家数据")]
    public int currentDay = 1;
    public string playerName = "主角";
    public int playerStamina = 100;
    public int playerSanity = 100;
    public int playerMoney = 50;

    [Header("游戏状态")]
    public string currentSceneName;
    public bool isPaused = false;
    public bool isInDialogue = false;

    // public List<Quest> quests = new List<Quest>();
    

    public void StartNewGame()
    {
        Debug.Log("开始新游戏，重置数据...");

        // 重置游戏数据
        currentDay = 1;
        // playerStamina = 100;
        // playerSanity = 100;
        // playerMoney = 50;

        // 加载开场场景
        LoadScene("OpeningScene");
    }

    public void ContinueGame()
    {
        Debug.Log("继续游戏...");

        if (SaveLoadManager.Instance != null)
        {
            SaveLoadManager.Instance.LoadGame();
            LoadScene(currentSceneName);
        }
        else
        {
            Debug.LogError("SaveLoadManager.Instance 为 null！");
            LoadScene("StartScene");
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("准备加载场景: " + sceneName);
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 等待一帧，确保所有Awake和Start执行完成
        yield return null;

        Debug.Log("正在加载场景: " + sceneName);
        SceneManager.LoadScene(sceneName);

        // 这行会在新场景中执行
        currentSceneName = sceneName;
        Debug.Log("场景加载完成: " + sceneName);
    }

    public void ConsumeStamina(int amount)
    {
        playerStamina -= amount;

        if (playerStamina <= 0)
        {
            playerStamina = 0;
            Debug.Log("体力耗尽，进入夜晚");
        }

        Debug.Log("消耗体力: " + amount + ", 剩余: " + playerStamina);
    }

    public void StartNewDay()
    {
        currentDay++;
        playerStamina = 100;

        Debug.Log("新的一天开始: 第" + currentDay + "天");

        if (currentDay > 7)
        {
            Debug.Log("触发Demo结局");
            LoadScene("DemoEndScene");
        }
    }

    public void TogglePause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;
        Debug.Log(pause ? "游戏暂停" : "游戏继续");
    }

    public void QuitGame()
    {
        Debug.Log("退出游戏");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}