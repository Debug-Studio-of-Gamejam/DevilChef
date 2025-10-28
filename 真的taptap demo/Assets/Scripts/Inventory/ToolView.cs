using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolView : MonoBehaviour
{
    public GameObject content;
    public ItemSlot itemSlot;

    private void OnEnable()
    {
        EventHandler.UpdateItemDetails += OnUpdateItemDetails;
    }

    private void OnDisable()
    {
        EventHandler.UpdateItemDetails -= OnUpdateItemDetails;
    }

    private void OnUpdateItemDetails(ItemDetails details)
    {
        Debug.Log("刷新itemUI ：" + details.name);
        itemSlot.SetTool(details);
    }
}
