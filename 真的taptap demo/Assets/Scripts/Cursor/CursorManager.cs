using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : Singleton<CursorManager>
{
    public Image cursor;
    ItemDetails currentItem;
    private Vector3 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private bool canClick = false;
    private bool holdItem = false;
    

    void Update()
    {

        canClick = ObjectAtMousePosition();

        if (holdItem)
        {
            cursor.rectTransform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.isPaused || GameManager.Instance.isTalking)
            {
                Debug.Log("游戏已停止");
                return;
            }
            
            if (canClick)
            {
                var go = ObjectAtMousePosition().gameObject;
                ClickAction(go);
            }
            if(holdItem)
            {
                EventHandler.CallItemSelectedEvent(null, false);
            }
            
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ClickAction(GameObject clickedObject)
    {
        switch (clickedObject?.tag)
        {
            case "Teleport":
                var teleport = clickedObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
            case "Item":
                var item = clickedObject.GetComponent<Item>();
                item?.ItemClicked();
                break;
            case "Ingredient":
                var ingredient = clickedObject.GetComponent<Ingredient>();
                ingredient?.IngredientClicked();
                break;
            case "Interactable":
                var interactable = clickedObject.GetComponent<Interactable>();
                if (holdItem)
                {
                    interactable?.CheckItem(currentItem.name);
                    //EventHandler.CallItemSelectedEvent(null, false);
                }
                else
                {
                    interactable?.Interact();
                }
                break;
        }
    }

    private void OnItemSelectedEvent(ItemDetails item, bool isSelected)
    {
        holdItem = isSelected;
        if (isSelected)
        {
            currentItem = item;
            cursor.sprite = item.icon;
            cursor.SetNativeSize();
        }
        cursor.gameObject.SetActive(isSelected);
    }

    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mousePos);
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }
}
