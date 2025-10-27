using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
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

        if (canClick && Input.GetMouseButtonDown(0))
        {
            var go = ObjectAtMousePosition().gameObject;
            ClickAction(go);
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
