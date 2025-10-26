using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private Vector3 mousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    private bool canClick = false;

    void Update()
    {
        canClick = ObjectAtMousePosition();
        if (canClick && Input.GetMouseButtonDown(0))
        {
            var go = ObjectAtMousePosition().gameObject;
            Debug.Log("点击：" + go.name);
            //检测点击交互
            ClickAction(go);

        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ClickAction(GameObject clickedObject)
    {
        switch (clickedObject.tag)
        {
            case "Teleport":
                var teleport = clickedObject.GetComponent<Teleport>();
                teleport?.TeleportToScene();
                break;
        }
    }

    private Collider2D ObjectAtMousePosition()
    {
        return Physics2D.OverlapPoint(mousePos);
    }
}
