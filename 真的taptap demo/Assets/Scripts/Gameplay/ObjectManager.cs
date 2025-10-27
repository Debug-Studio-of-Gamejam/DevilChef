using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// //保存场景中所有Object（工具，食材，交互物）的状态
/// </summary>
public class ObjectManager : Singleton<ObjectManager>
{
    //Item只有一个，ingredient可能有很多个，按照gameObject.name记录
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> ingredientAvailableDict = new Dictionary<string, bool>();
    private Dictionary<string, bool> interactableDict = new Dictionary<string, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnload;
        EventHandler.AfterSceneLoadEvent += OnAfterSceneLoad;
        EventHandler.UpdateItemDetails += OnPickItem;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnload;
        EventHandler.AfterSceneLoadEvent -= OnAfterSceneLoad;
        EventHandler.UpdateItemDetails -= OnPickItem;
    }

    void OnPickItem(ItemDetails details)
    {
        if (details != null && itemAvailableDict.ContainsKey(details.name))
        {
            itemAvailableDict[details.name] = false;
        }
    }
    
    //
    public void OnPickIngredient(string objectName)
    {
        Debug.Log("OnPickIngredient : " + objectName);
        var name = TransitionManager.Instance.CurrentSceneName + objectName;
        if (ingredientAvailableDict.ContainsKey(name))
        {
            ingredientAvailableDict[name] = false;
        }
    }

    void OnBeforeSceneUnload()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
            {
                itemAvailableDict.Add(item.itemName, true);
            }
        }
        foreach (var i in FindObjectsOfType<Ingredient>())
        {
            var name = TransitionManager.Instance.CurrentSceneName + i.gameObject.name;
            if (!ingredientAvailableDict.ContainsKey(name))
            {
                //场景中Object发生变化时候，出场景前保存状态
                ingredientAvailableDict.Add(name, true);
            }
        }
    }

    void OnAfterSceneLoad()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            
            if (!itemAvailableDict.ContainsKey(item.itemName))
            {
                //第一次加载场景所有Object都显示
                itemAvailableDict.Add(item.itemName, true);
            }
            else
            {
                //之后再进入场景，就根据已保存的状态控制Object的显示
                item.gameObject.SetActive(itemAvailableDict[item.itemName]);
            }
        }
        
        foreach (var i in FindObjectsOfType<Ingredient>())
        {
            var name = TransitionManager.Instance.CurrentSceneName + i.gameObject.name;
            if (!ingredientAvailableDict.ContainsKey(name))
            {
                ingredientAvailableDict.Add(name, true);
            }
            else
            {
                i.gameObject.SetActive(ingredientAvailableDict[name]);
            }
        }
    }
}
