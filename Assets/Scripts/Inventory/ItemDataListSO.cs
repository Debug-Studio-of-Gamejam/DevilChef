using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList", menuName = "Inventory/ItemDataList")]
public class ItemDataListSO : ScriptableObject
{
    public List<ItemDetails> itemDetailsList = new List<ItemDetails>();

    public ItemDetails GetItemDetails(ItemName itemName)
    {
        return itemDetailsList.Find(i => i.name == itemName);
    }

}

[System.Serializable]
public class ItemDetails
{
    public ItemName name;
    public Sprite icon;
    //public bool isTool;
    public string description;
}