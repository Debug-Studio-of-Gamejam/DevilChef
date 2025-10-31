using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

// 挂载到你的 CookingCanvas (根物体) 上
[RequireComponent(typeof(GraphicRaycaster))]
public class RaycastDebugger : MonoBehaviour
{
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    void Start()
    {
        // 获取 Canvas 上的“雷达”
        m_Raycaster = GetComponent<GraphicRaycaster>();

        // 获取 EventSystem
        m_EventSystem = FindObjectOfType<EventSystem>();

        if (m_EventSystem == null)
        {
            Debug.LogError("RaycastDebugger: 找不到 EventSystem!");
        }
    }

    void Update()
    {
        if (m_Raycaster == null || m_EventSystem == null) return;

        // 1. 设置一个“虚拟”的鼠标指针事件
        m_PointerEventData = new PointerEventData(m_EventSystem);

        // 2. 设置鼠标位置
        m_PointerEventData.position = Input.mousePosition;

        // 3. 存储“雷达”扫描到的所有物体
        List<RaycastResult> results = new List<RaycastResult>();

        // 4. !! 核心：手动执行“雷达”扫描 !!
        m_Raycaster.Raycast(m_PointerEventData, results);

        // 5. 打印出“雷达”扫描到的第一个物体
        if (results.Count > 0)
        {
            // 我们只关心最上层的那个物体
            Debug.Log("--- RaycastDebugger 看到: " + results[0].gameObject.name);
        }
        else
        {
            // 鼠标悬停在空处
            Debug.Log("--- RaycastDebugger 看到: NULL ---");
        }
    }
}