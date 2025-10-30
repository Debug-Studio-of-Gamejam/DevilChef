using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    [SceneName] public string mapSceneName;
    public List<Sprite> roundNumSprites;
    
    public Image roundNumImage;
    public GameObject backButton;

    public void UpdateRoundNum()
    {
        Debug.Log($"更新轮数 : {GameManager.Instance.currentRound}");
        if (GameManager.Instance.currentRound >= 1 && GameManager.Instance.currentRound <=7)
        {
            roundNumImage.sprite = roundNumSprites[GameManager.Instance.currentRound-1];
            roundNumImage.SetNativeSize();
        }
    }

    private void Start()
    {
        backButton.GetComponent<Button>()?.onClick.AddListener(() => TransitionManager.Instance.Transition(TransitionManager.Instance.CurrentSceneName, mapSceneName));
    }

    public void ToggleBackButton(bool isOn)
    {
        backButton.SetActive(isOn);
    }
}
