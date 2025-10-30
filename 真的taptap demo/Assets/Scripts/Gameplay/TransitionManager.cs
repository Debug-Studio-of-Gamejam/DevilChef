using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName] public string startSceneName;
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    bool isFading = false;
    
    public string CurrentSceneName { get; private set; }

    private void Start()
    {
        Transition(string.Empty, startSceneName);
    }

    public void Transition(string fromSceneName, string toSceneName)
    {
        if (!isFading)
            StartCoroutine(TransitionToScene(fromSceneName, toSceneName));
    }

    private IEnumerator TransitionToScene(string fromSceneName, string toSceneName)
    {
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
        
        // 检查canvasGroup是否被销毁
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup已被销毁，跳过淡入淡出效果");
            isFading = false;
            yield break;
        }
        
        canvasGroup.blocksRaycasts = true;
        
        float speed = Mathf.Abs(canvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            // 每次循环都检查canvasGroup是否还存在
            if (canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup在淡入淡出过程中被销毁");
                break;
            }
            
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
        isFading = false;
        
    }
}
