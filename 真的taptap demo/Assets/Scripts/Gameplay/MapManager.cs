using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject mapPanel;
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
        mapPanel.SetActive(newSceneName == mapSceneName || subMapSceneNames.Contains(newSceneName));
    }

    void HideMapview()
    {
        mapPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += UpdateMapview;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= UpdateMapview;
    }
    
}
