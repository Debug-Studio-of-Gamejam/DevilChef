using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapView : MonoBehaviour
{
    [SceneName] public string mapSceneName;
    [SceneName] public List<string> subMapSceneNames;
    public GameObject backButton;

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += UpdateReturnButton;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= UpdateReturnButton;
    }

    void UpdateReturnButton()
    {
        backButton.SetActive(subMapSceneNames.Contains(TransitionManager.Instance.CurrentSceneName));
    }

    public void onClickReturnButton()
    {
        TransitionManager.Instance.Transition(TransitionManager.Instance.CurrentSceneName, mapSceneName);
    }

    
}
