using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [SceneName] public string mapSceneName;
    public List<Sprite> dayNumSprites;
    
    public Image dayNumImage;
    public GameObject backButton;
    
    private void Start()
    {
        backButton.GetComponent<Button>()?.onClick.AddListener(() => TransitionManager.Instance.Transition(TransitionManager.Instance.CurrentSceneName, mapSceneName));
    }

    public void ToggleBackButton(bool isOn)
    {
        backButton.SetActive(isOn);
    }
}
