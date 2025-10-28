using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI组件")]
    public Image backgroundImage;      // 格子背景
    public Image iconImage;            // 物品图标
    public Image amountBackground;     // 数量背景
    public TextMeshProUGUI amountText; // 数量文本

    [Header("状态")]
    bool isSelected = false;

    private IngredientEntry currentIngredient;

    // 设置为食材道具
    public void SetIngredient(IngredientEntry entry, Sprite icon)
    {
        // IngredientEntry是结构体，不能与null比较
        // 检查是否为默认值（空食材）
        if (entry.name == IngredientName.None) return;
        
        currentIngredient = entry;

        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.color = Color.white;
        }

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
        // 检查currentIngredient是否为有效食材
        if (currentIngredient.name == IngredientName.None || InventoryManager.Instance == null) return;
        
        isSelected = !isSelected;

        // 食材选择逻辑 - 显示食材详情
        var ingredientDetails = InventoryManager.Instance.ingredientData.GetItemDetails(currentIngredient.name);
        if (ingredientDetails != null)
        {
            // 创建临时的ItemDetails对象来显示食材信息
            var tempItemDetails = new ItemDetails
            {
                name = ItemName.None, // 食材不使用ItemName枚举，设为None
                icon = ingredientDetails.icon,
                description = ingredientDetails.description
            };
            EventHandler.CallItemSelectedEvent(tempItemDetails, isSelected);
        }
    }

    // TODO: 显示道具说明
    public void OnPointerEnter(PointerEventData eventData)
    {
        string description = InventoryManager.Instance.ingredientData.GetItemDetails(currentIngredient.name).description;

        // TODO: 实现工具提示显示逻辑
        Debug.Log($"显示物品信息: {description}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO: 实现工具提示隐藏逻辑
        Debug.Log("隐藏物品信息");
    }
}