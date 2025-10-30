using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName] public string startSceneName;
    [SceneName] public string OpenningSceneName; 
    [SceneName] public string gameEndSceneName;
    [SceneName] public string mapSceneName;
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    bool isFading = false;
    
    public string CurrentSceneName { get; private set; }

    private void Start()
    {
        Transition(string.Empty, startSceneName);
    }
    
    public void ReturnToStartScene()
    {
        Transition(CurrentSceneName, startSceneName);
    }
    
    public void TransitionToOpenning()
    {
        if(CurrentSceneName == startSceneName)
            Transition(CurrentSceneName, OpenningSceneName);
        else
        {
            Debug.LogWarning($"从 {CurrentSceneName} 进入开场动画");
        }
    }

    public void BackToMapScene()
    {
        Transition(CurrentSceneName, mapSceneName);
    }
    
    public void TransitionToEndScene()
    {
        Transition(CurrentSceneName, gameEndSceneName);
    }

    public void Transition(string fromSceneName, string toSceneName)
    {
        if (!isFading)
            StartCoroutine(TransitionToScene(fromSceneName, toSceneName));
    }

    private IEnumerator TransitionToScene(string fromSceneName, string toSceneName)
    {
        Debug.Log($"开始切换场景 {fromSceneName} -> {toSceneName}");
        yield return Fade(1);
        if (fromSceneName != string.Empty)
        {
            EventHandler.CallBeforeSceneUnloadEvent(fromSceneName); 
            yield return SceneManager.UnloadSceneAsync(fromSceneName);
        }
        
        yield return SceneManager.LoadSceneAsync(toSceneName, LoadSceneMode.Additive);

        CurrentSceneName = toSceneName;
        Scene newScene = SceneManager.GetSceneByName(toSceneName);
        SceneManager.SetActiveScene(newScene);
        
        EventHandler.CallAfterSceneLoadEvent(toSceneName);
        yield return Fade(0);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true;
        
        float speed = Mathf.Abs(canvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
        isFading = false;
        
    }


}
