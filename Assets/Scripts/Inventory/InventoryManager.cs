using System;
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
    public List<ItemName> items = new List<ItemName>();
    public List<IngredientEntry> ingredients = new List<IngredientEntry>();
    private void OnEnable()
    {
        EventHandler.StartNewGameEvent += OnStartNewGame;
    }

    private void OnDisable()
    {
        EventHandler.StartNewGameEvent -= OnStartNewGame;
    }

    private void OnStartNewGame()
    {
        items.Clear();
        ingredients.Clear();
    }

    public void AddItem(ItemName itemName)
    {
        if (!items.Contains(itemName))
        {
            items.Add(itemName);
            EventHandler.CallGetNewItemEvent(itemName);
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
    public void ConsumeIngredient(IngredientName name, int amount = 1)
    {
        var i = ingredients.FindIndex(e => e.name == name);
        if (i != -1)
        {
            int newAmount = ingredients[i].amount - amount;
            if (newAmount > 0)
            {
                ingredients[i] = new IngredientEntry { name = name, amount = newAmount };
            }
            else
            {
                ingredients.RemoveAt(i);
            }
        }
    }

    
}
