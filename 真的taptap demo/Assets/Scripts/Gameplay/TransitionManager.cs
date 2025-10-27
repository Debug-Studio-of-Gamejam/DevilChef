using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : Singleton<TransitionManager>
{
    public CanvasGroup canvasGroup;
    public GameObject backButton;
    public float fadeDuration;
    bool isFading = false;
    [SceneName] public string mapSceneName;
    string currentSceneName;
    
    public void Transition(string fromSceneName, string toSceneName)
    {
        if (!isFading)
            StartCoroutine(TransitionToScene(fromSceneName, toSceneName));
    }

    public void BackToMap()
    {
        if (currentSceneName != mapSceneName)
        {
            Transition(currentSceneName, mapSceneName);
        }
    }

    private IEnumerator TransitionToScene(string fromSceneName, string toSceneName)
    {
        yield return Fade(1);
        EventHandler.CallBeforeSceneUnloadEvent();
        
        yield return SceneManager.UnloadSceneAsync(fromSceneName);
        yield return SceneManager.LoadSceneAsync(toSceneName, LoadSceneMode.Additive);

        currentSceneName = toSceneName;
        Scene newScene = SceneManager.GetSceneByName(toSceneName);
        SceneManager.SetActiveScene(newScene);
        //TODO: 整理一下Map相关的逻辑
        backButton.SetActive(currentSceneName != mapSceneName);
        
        EventHandler.CallAfterSceneLoadEvent();
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
