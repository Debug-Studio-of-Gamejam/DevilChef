using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject toolPanel;
    public GameObject inventoryButton;
    public GameObject TipsPanel;
    [SceneName] public string mapSceneName;
    [SceneName] public List<string> subMapSceneNames;
    MapView mapView;
    ToolView toolView;
    

    private void Start()
    {
        mapView = mapPanel.GetComponent<MapView>();
        toolView = toolPanel.GetComponent<ToolView>();
    }

    void UpdateMapview(string newSceneName)
    {
        bool isExploring = newSceneName == mapSceneName || subMapSceneNames.Contains(newSceneName);
        bool isInSubMap = subMapSceneNames.Contains(newSceneName);
        inventoryButton.SetActive(isExploring);
        mapPanel.SetActive(isExploring);
        toolPanel.SetActive(isInSubMap);
        mapView.ToggleBackButton(isInSubMap);
        if (isExploring)
        {
            mapView.UpdateRoundNum();
            toolView.UpdateItemSlots();
        }
    }

    void OnStartDialogue(int id)
    {
        var currentScene = TransitionManager.Instance.CurrentSceneName;
        if (currentScene == mapSceneName || subMapSceneNames.Contains(currentScene)){
            inventoryButton.SetActive(false);
            toolPanel.SetActive(false);
        }
    }
    
    void OnFinishDialogue(int id)
    {
        var currentScene = TransitionManager.Instance.CurrentSceneName;
        if (currentScene == mapSceneName || subMapSceneNames.Contains(currentScene)){
            inventoryButton.SetActive(true);
            toolPanel.SetActive(true);
            toolView.UpdateItemSlots();
        }
    }
    

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += UpdateMapview;
        EventHandler.DialogueStartdEvent += OnStartDialogue;
        EventHandler.DialogueFinishedEvent += OnFinishDialogue;

    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= UpdateMapview;
        EventHandler.DialogueStartdEvent -= OnStartDialogue;
        EventHandler.DialogueFinishedEvent -= OnFinishDialogue;
    }
    
}
