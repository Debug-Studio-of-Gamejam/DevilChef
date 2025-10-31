using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolListTest : MonoBehaviour
{
    public void GetItem()
    {
        ItemName[] values = (ItemName[])System.Enum.GetValues(typeof(ItemName));

        int startIndex = 1;
        // 从 1 到 values.Length - 1 随机一个
        int index = Random.Range(startIndex, values.Length);
        InventoryManager.Instance.AddItem(values[index]);
        Debug.Log($"获得道具：{ values[index] }");
    }
}
