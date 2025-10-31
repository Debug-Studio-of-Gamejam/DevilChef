using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolView : MonoBehaviour
{
    public GameObject content;
    public List<ItemSlot> itemSlots;

    private void Start()
    {
        itemSlots = content.GetComponentsInChildren<ItemSlot>(includeInactive: true).ToList();
        UpdateItemSlots();
    }

    public void UpdateItemSlots()
    {
        HideAllItemSlots();
        for (int i = 0; i < InventoryManager.Instance.items.Count && i < itemSlots.Count; i++)
        {
            var itemName = InventoryManager.Instance.items[i];
            var details= InventoryManager.Instance.itemData.GetItemDetails(itemName);
            itemSlots[i].SetItem(details);
            itemSlots[i].gameObject.SetActive(true);
        }
    }

    private void HideAllItemSlots()
    {
        foreach (var slot in itemSlots)
        {
            slot.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHandler.GetNewItemEvent += OnGetNewItem;
    }

    private void OnDisable()
    {
        EventHandler.GetNewItemEvent -= OnGetNewItem;
    }

    private void OnGetNewItem(ItemName itemName)
    {
        UpdateItemSlots();
    }
}
