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
    public bool isDone;

    private void Start()
    {
        if (characterName != CharacterName.None)
        {
            characterEvent = DataLoader.Instance.characterEvents[characterName];
        }
    }

    public void CheckItem(ItemName item)
    {
        if (item == characterEvent.requiredItem)
        {
            if (!isDone)
            {
                isDone = true;
                OnClickAction();
            }
            else
            {
                DialogueSystem.Instance.ShowMessage($" 已经对“{characterName}”使用过了“{item}”，无法再获得更多”{characterEvent.rewardIngredient}“");
            }
        }
        else
        {
            DialogueSystem.Instance.ShowMessage($"无法对“{characterName}”使用“{item}”");
        }
        
        
    }

    /// <summary>
    /// 正确使用道具时候执行
    /// </summary>
    public virtual void OnClickAction()
    {
        InventoryManager.Instance.AddIngredient(characterEvent.rewardIngredient);
        DialogueSystem.Instance.ShowMessage($"获得了“{characterEvent.rewardIngredient}”");;
    }

    /// <summary>
    /// 没有选择道具直接点击
    /// </summary>
    public virtual void Interact()
    {
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
