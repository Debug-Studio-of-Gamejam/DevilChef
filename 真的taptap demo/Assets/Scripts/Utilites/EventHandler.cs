using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static event Action<ItemDetails> UpdateItemDetails;

    public static void CallUpdateItemDetails(ItemDetails details)
    {
        UpdateItemDetails?.Invoke(details);
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
    
    public static event Action UpdateIngredients;

    public static void CallUpdateIngredients()
    {
        UpdateIngredients?.Invoke();
    }
    
    public static event Action<int> DialogueFinishedEvent;

    public static void CallIDialogueFinishedEvent(int dialogueID)
    {
        DialogueFinishedEvent?.Invoke(dialogueID);
    }
    

}
