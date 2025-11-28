using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenningScene : MonoBehaviour
{
    // 开场剧情有2条对话，两段对话背景不同。第一条显示宫殿，第二条显示冥府主殿
    public int dialogueId1;
    public int dialogueId2;
    public int endDialogueIds;
    
    public Sprite newSprite;
    [SceneName] public string fromSceneName;
    [SceneName] public string toSceneName;
    
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        DialogueSystem.Instance.ShowDialogue(dialogueId1);
    }

    void ShowDialogueNext(int lasrDialogueId)
    {
        Debug.Log($"对话 {lasrDialogueId} 结束了");
        if (lasrDialogueId == dialogueId1)
        {
            spriteRenderer.sprite = newSprite;
            DialogueSystem.Instance.ShowDialogue(dialogueId2);
        }

        if (lasrDialogueId == endDialogueIds)
        {
            TransitionManager.Instance.Transition(fromSceneName, toSceneName);
        }
    }

    private void OnEnable()
    {
        EventHandler.DialogueFinishedEvent += ShowDialogueNext;
    }

    private void OnDisable()
    {
        EventHandler.DialogueFinishedEvent -= ShowDialogueNext;
    }
}
