using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemTips itemTips;
    public Image image;
    public ItemDetails currentItemDetails;
    // bool isSelected = false;
    
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
        // isSelected = !isSelected;
        EventHandler.CallItemSelectedEvent(currentItemDetails, true);
    }

    // TODO: 显示道具说明
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemTips.UpdateItemTips(currentItemDetails);
        itemTips.transform.position = new Vector3(itemTips.transform.position.x, this.transform.position.y, 0);
        itemTips.gameObject.SetActive(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTips.gameObject.SetActive(false);
    }
}
