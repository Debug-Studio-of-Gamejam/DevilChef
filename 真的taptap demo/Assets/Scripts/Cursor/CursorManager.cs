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
    private Vector3 mousePos
    {
        get
        {
            // 首先尝试使用Camera.main
            if (Camera.main != null)
            {
                return Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            
            // 如果Camera.main为null，尝试查找可用的摄像机
            Debug.LogWarning("主摄像机为null，尝试查找可用的摄像机");
            Camera[] cameras = FindObjectsOfType<Camera>();
            
            // 优先查找标记为MainCamera的启用摄像机
            foreach (Camera cam in cameras)
            {
                if (cam.CompareTag("MainCamera") && cam.isActiveAndEnabled)
                {
                    Debug.Log("找到并使用标记为MainCamera的摄像机: " + cam.name);
                    return cam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            
            // 如果没有找到MainCamera，使用第一个启用的摄像机
            foreach (Camera cam in cameras)
            {
                if (cam.isActiveAndEnabled)
                {
                    Debug.LogWarning("未找到标记为MainCamera的摄像机，使用第一个启用的摄像机: " + cam.name);
                    return cam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            
            // 如果没有任何摄像机可用，返回默认值但不报错
            Debug.LogWarning("未找到可用的摄像机，返回默认位置");
            return Vector3.zero;
        }
    }
    private bool canClick = false;
    private bool holdItem = false;
    

    void Update()
    {
        if (GameManager.Instance.isPaused || GameManager.Instance.isTalking)
        {
            return;
        }
        canClick = ObjectAtMousePosition();

        if (holdItem)
        {
            cursor.rectTransform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
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
