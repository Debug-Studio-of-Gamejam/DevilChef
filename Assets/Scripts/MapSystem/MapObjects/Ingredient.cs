using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public IngredientName ingredientName;
    
    public void IngredientClicked()
    {
        ObjectManager.Instance.OnPickIngredient(this.gameObject.name);
        InventoryManager.Instance.AddIngredient(ingredientName, 1);
        this.gameObject.SetActive(false);
    }
}
