using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    public Image image;
    public ItemDetails currentItemDetails;
    
    // TODO： 改用 toggle group来管理吗?
    bool isSelected = false;

    public void SetItem(ItemDetails itemDetails)
    {
        currentItemDetails = itemDetails;
        image.sprite = itemDetails.icon;
        image.SetNativeSize();
    }

    public void SetEmpty()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        EventHandler.CallItemSelectedEvent(currentItemDetails, isSelected);
    }

    // TODO: 显示道具说明
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
