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
    
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }
    
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }
    
    public static event Action<ItemDetails,bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails details, bool selected)
    {
        ItemSelectedEvent.Invoke(details, selected);
    }
}
