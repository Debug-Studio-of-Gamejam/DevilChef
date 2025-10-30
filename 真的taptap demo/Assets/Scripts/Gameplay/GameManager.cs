using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

    [Header("玩家数据")]
    public int currentRound = 1;
    public string playerName = "你";

    [Header("游戏状态")]
    public string currentSceneName;
    public bool isPaused = false;
    public bool isTalking = false;
    public HashSet<int> triggeredDialogues = new HashSet<int>();

    [Header("游戏结束")]
    public bool isSuccess;

    public void StartNewGame()
    {
        currentRound = 1;
        triggeredDialogues.Clear();
        isPaused = false;
        EventHandler.CallStartNewGame();
        TransitionManager.Instance.TransitionToOpenning();
    }

    public void ContinueGame()
    {
        isPaused = false;
        TransitionManager.Instance.BackToMapScene();
    }

    public void OnScoreEvaluated(bool success)
    {
        isPaused = true;
        isSuccess = success;
        TransitionManager.Instance.TransitionToEndScene();
    }


    public void StartNewRound()
    {
        currentRound++;
        isPaused = false;
        TransitionManager.Instance.BackToMapScene();
        Debug.Log("新的一天开始: 第" + currentRound + "天");

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