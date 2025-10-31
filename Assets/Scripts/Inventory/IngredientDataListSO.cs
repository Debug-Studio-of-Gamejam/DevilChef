using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDataList", menuName = "Inventory/IngredientDataList")]
public class IngredientDataListSO : ScriptableObject
{
    public List<IngredientDetails> itemDetailsList = new List<IngredientDetails>();

    public IngredientDetails GetItemDetails(IngredientName name)
    {
        return itemDetailsList.Find(i => i.name == name);
    }

}

[System.Serializable]
public class IngredientDetails
{
    public IngredientName name;
    public Sprite icon;
    public string description;
    //public int maxAmount;
}