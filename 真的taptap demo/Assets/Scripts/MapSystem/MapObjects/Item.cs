using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemName itemName;

    public void ItemClicked()
    {
        InventoryManager.Instance.AddItem(itemName);
        
        // TODO:场景中不会直接出现Item
        this.gameObject.SetActive(false);
    }
}
