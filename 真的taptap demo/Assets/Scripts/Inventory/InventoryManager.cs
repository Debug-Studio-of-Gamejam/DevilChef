using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct IngredientEntry
{
    public IngredientName name;
    public int amount;
}
public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataListSO itemData;
    public IngredientDataListSO ingredientData;
    
    //玩家拥有的道具和食材
    public List<ItemName> items;
    public List<IngredientEntry> ingredients = new();
    
    public void AddItem(ItemName itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            EventHandler.CallUpdateItemDetails(itemData.GetItemDetails(itemName));
        }
    }
    
    public void AddIngredient(IngredientName name, int amount = 1)
    {
        var i = ingredients.FindIndex(e => e.name == name);
        if (i == -1)
            ingredients.Add(new IngredientEntry { name = name, amount = amount });
        else
            ingredients[i] = new IngredientEntry { name = name, amount = ingredients[i].amount + amount };
    }
    
    /// <summary>
    /// 移除食材
    /// </summary>
    public void RemoveIngredient(IngredientName name, int amount = 1)
    {
        var i = ingredients.FindIndex(e => e.name == name);
        if (i != -1)
        {
            int newAmount = ingredients[i].amount - amount;
            if (newAmount <= 0)
            {
                // 如果数量为0或负数，移除该食材
                ingredients.RemoveAt(i);
            }
            else
            {
                // 更新数量
                ingredients[i] = new IngredientEntry { name = name, amount = newAmount };
            }
        }
    }
}
