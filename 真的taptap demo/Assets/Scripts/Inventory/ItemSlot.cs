using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI组件")]
    public Image backgroundImage;      // 格子背景
    public Image iconImage;            // 物品图标
    public Image amountBackground;     // 数量背景
    public TextMeshProUGUI amountText; // 数量文本

    [Header("状态")]
    bool isSelected = false;

    private ItemDetails currentItem;
    private IngredientEntry currentIngredient;
    private bool isTool; // true=工具, false=食材

    // 设置为工具道具  
    public void SetTool(ItemDetails itemDetails)
    {
        isTool = true;
        currentItem = itemDetails;

        iconImage.sprite = itemDetails.icon;
        iconImage.color = Color.white;

        if (amountText != null)
            amountText.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    // 设置为食材道具
    public void SetIngredient(IngredientEntry entry, Sprite icon)
    {
        isTool = false;
        currentIngredient = entry;

        iconImage.sprite = icon;
        iconImage.color = Color.white;

        if (amountText != null)
        {
            // 数量为1时不显示，大于1时显示"*数量"格式
            if (entry.amount > 1)
            {
                amountText.text = $"*{entry.amount}";
                amountText.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }
        }

        gameObject.SetActive(true);
    }

    // 设置为空槽位
    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;

        if (isTool)
        {
            EventHandler.CallItemSelectedEvent(currentItem, isSelected);
        }
        else
        {
            // 食材选择逻辑 - 显示食材详情
            var ingredientDetails = InventoryManager.Instance.ingredientData.GetItemDetails(currentIngredient.name);
            if (ingredientDetails != null)
            {
                // 创建临时的ItemDetails对象来显示食材信息
                var tempItemDetails = new ItemDetails
                {
                    name = ItemName.None, // 食材不使用ItemName枚举
                    icon = ingredientDetails.icon,
                    description = ingredientDetails.description
                };
                EventHandler.CallItemSelectedEvent(tempItemDetails, isSelected);
            }
        }
    }

    // TODO: 显示道具说明
    public void OnPointerEnter(PointerEventData eventData)
    {
        string description = isTool ? currentItem.description :
            InventoryManager.Instance.ingredientData.GetItemDetails(currentIngredient.name).description;

        // TODO: 实现工具提示显示逻辑
        Debug.Log($"显示物品信息: {description}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: 实现工具提示隐藏逻辑
        Debug.Log("隐藏物品信息");
    }
}
