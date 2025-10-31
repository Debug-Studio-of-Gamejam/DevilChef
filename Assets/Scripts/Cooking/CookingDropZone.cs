using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookingDropZone : MonoBehaviour, IDropHandler
{
    private CookingUI cookingUI;

    void Awake()
    {
        cookingUI = GetComponentInParent<CookingUI>();
        if (cookingUI == null)
        {
            Debug.LogError("CookingDropZone找不到CookingUI脚本");
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (CookingBackpackSlot.ingredientBeingDragged != IngredientName.None)
        {
            bool success = cookingUI.TryAddIngredient
            (
                CookingBackpackSlot.ingredientBeingDragged,
                CookingBackpackSlot.spriteBeingDragged
            );
            if (success)
            {
                InventoryManager.Instance.ConsumeIngredient(CookingBackpackSlot.ingredientBeingDragged, 1);
                cookingUI.RefreshBackpackDisplay();
                CookingBackpackSlot.dropWasSuccessful = true;
            }
            else
            {
                CookingBackpackSlot.dropWasSuccessful = false;
            }
        }
    }
}