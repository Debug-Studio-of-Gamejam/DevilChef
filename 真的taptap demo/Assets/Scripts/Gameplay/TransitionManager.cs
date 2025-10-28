using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TransitionManager : Singleton<TransitionManager>
{
    [SceneName] public string StartSceneName;
    public CanvasGroup canvasGroup;
    public float fadeDuration;
    bool isFading = false;
    
    public string CurrentSceneName { get; private set; }

    private void Start()
    {
        Transition(string.Empty, StartSceneName);
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
            EventHandler.CallBeforeSceneUnloadEvent(); 
            yield return SceneManager.UnloadSceneAsync(fromSceneName);
        }
        
        yield return SceneManager.LoadSceneAsync(toSceneName, LoadSceneMode.Additive);

        CurrentSceneName = toSceneName;
        Scene newScene = SceneManager.GetSceneByName(toSceneName);
        SceneManager.SetActiveScene(newScene);
        
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
