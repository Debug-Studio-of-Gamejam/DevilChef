using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject mapPanel;
    public GameObject toolPanel;
    [SceneName] public string mapSceneName;
    [SceneName] public List<string> subMapSceneNames;
    MapView mapView;

    private void Start()
    {
        mapView = mapPanel.GetComponent<MapView>();
    }

    void UpdateMapview(string newSceneName)
    {
        mapView.ToggleBackButton(subMapSceneNames.Contains(newSceneName));
        bool isExploring = newSceneName == mapSceneName || subMapSceneNames.Contains(newSceneName);
        mapPanel.SetActive(isExploring);
        toolPanel.SetActive(isExploring);
    }

    void OnStartDialogue(int id)
    {
        toolPanel.SetActive(false);
    }
    
    void OnFinishDialogue(int id)
    {
        toolPanel.SetActive(true);
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
