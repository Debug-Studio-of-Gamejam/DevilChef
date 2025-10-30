using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static event Action<ItemName> GetNewItemEvent;

    public static void CallGetNewItemEvent(ItemName itemName)
    {
        GetNewItemEvent?.Invoke(itemName);
    }
    
    public static event Action<string> BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent(string oldScene)
    {
        BeforeSceneUnloadEvent?.Invoke(oldScene);
    }
    
    public static event Action<string> AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent(string newScene)
    {
        AfterSceneLoadEvent?.Invoke(newScene);
    }
    
    public static event Action<ItemDetails,bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails details, bool selected)
    {
        ItemSelectedEvent?.Invoke(details, selected);
    }
    
    public static event Action<int> DialogueStartdEvent;

    public static void CallIDialogueStartEvent(int dialogueID)
    {
        DialogueStartdEvent?.Invoke(dialogueID);
    }
    public static event Action<int> DialogueFinishedEvent;

    public static void CallIDialogueFinishedEvent(int dialogueID)
    {
        DialogueFinishedEvent?.Invoke(dialogueID);
    }
    
    public static event Action<int> SelectDialogueOptionEvent;

    public static void CallISelectDialogueOptionEvent(int optionId)
    {
        SelectDialogueOptionEvent?.Invoke(optionId);
    }
    
    public static event Action StartNewGameEvent;

    public static void CallStartNewGame()
    {
        StartNewGameEvent?.Invoke();
    }
    

}
