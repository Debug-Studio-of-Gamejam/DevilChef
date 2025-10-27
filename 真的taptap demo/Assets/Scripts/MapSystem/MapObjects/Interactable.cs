using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public ItemName requiredItem;
    public bool isDone;

    public void CheckItem(ItemName item)
    {
        if (item == requiredItem && !isDone)
        {
            isDone = true;
            OnClickAction();
        }
        //TODO : 错误的物品
    }

    /// <summary>
    /// 正确使用道具时候执行
    /// </summary>
    public virtual void OnClickAction()
    {
        Debug.Log("对 [" + gameObject.name +  "] 正确使用道具 : " + requiredItem);
        EventHandler.CallItemSelectedEvent(null, false);
    }

    /// <summary>
    /// 没有选择道具直接点击
    /// </summary>
    public virtual void Interact()
    {
        Debug.Log("没有选择道具直接点击 :" + gameObject.name);
    }

}
