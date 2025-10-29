using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Interactable : MonoBehaviour
{
    [Header("角色对话")]
    public CharacterName characterName;
    private CharacterEvent characterEvent;
    
    [Header("物品交互")]
    public ItemName requiredItem;
    public bool isDone;

    private void Start()
    {
        characterEvent = DataLoader.Instance.characterEvents[characterName];
    }

    public void CheckItem(ItemName item)
    {
        if (item == requiredItem && !isDone)
        {
            isDone = true;
            OnClickAction();
        }
        //TODO : 错误的物品
    }

    /// <summary>
    /// 正确使用道具时候执行
    /// </summary>
    public virtual void OnClickAction()
    {
        Debug.Log("对 [" + gameObject.name +  "] 正确使用道具 : " + requiredItem);
        EventHandler.CallItemSelectedEvent(null, false);
    }

    /// <summary>
    /// 没有选择道具直接点击
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log($"点击 {characterName}");
        if (characterEvent == null)
        {
            Debug.LogWarning($"Interactable {characterName} 没有关联的 CharacterEvent。");
            return;
        }

        int dialogueToShow = DetermineDialogueID();
        DialogueSystem.Instance.ShowDialogue(dialogueToShow);
    }
    
    private int DetermineDialogueID()
    {
        // 1. 如果当前轮次 == 条件轮次
        var currentRound = GameManager.Instance.currentRound;
        if (currentRound == characterEvent.conditionRound)
        {
            if (GameManager.Instance.triggeredDialogues.Contains(characterEvent.requiredDialogueID))
            {
                return characterEvent.successDialogueID;
            }
        }

        // 2. 如果当前轮次在 specialRounds 中
        if (characterEvent.specialRounds.Contains(currentRound))
        {
            return characterEvent.specialDialogueID;
        }

        // 3. 否则返回 normalDialogueID
        return characterEvent.normalDialogueID;
    }
}
