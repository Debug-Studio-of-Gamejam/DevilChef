using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包系统测试脚本
/// </summary>
public class InventoryTest : MonoBehaviour
{
    [Header("测试按钮")]
    public Button testAddButton; // 添加测试物品按钮
    public Button testRemoveButton; // 删除测试物品按钮
    public Button testToggleButton; // 切换背包显示按钮
    
    private InventoryPanel inventoryPanel;
    
    void Start()
    {
        // 查找背包面板
        inventoryPanel = FindObjectOfType<InventoryPanel>();
        
        if (inventoryPanel != null)
        {
            Debug.Log("找到背包面板");
            
            // 绑定测试按钮事件
            if (testAddButton != null)
                testAddButton.onClick.AddListener(TestAddItem);
                
            if (testRemoveButton != null)
                testRemoveButton.onClick.AddListener(TestRemoveItem);
                
            if (testToggleButton != null)
                testToggleButton.onClick.AddListener(TestToggleInventory);
        }
        else
        {
            Debug.LogWarning("未找到背包面板");
        }
    }
    
    /// <summary>
    /// 测试添加物品
    /// </summary>
    private void TestAddItem()
    {
        if (InventoryManager.Instance != null)
        {
            // 添加一些测试食材（使用项目中定义的食材名称）
            InventoryManager.Instance.AddIngredient(IngredientName.香槟酒, 5);
            
            Debug.Log("添加测试物品完成");
            
            // 触发更新事件
            EventHandler.CallUpdateIngredients();
        }
    }
    
    /// <summary>
    /// 测试删除物品
    /// </summary>
    private void TestRemoveItem()
    {
        if (InventoryManager.Instance != null)
        {
            // 删除一些物品
            InventoryManager.Instance.RemoveIngredient(IngredientName.香槟酒, 2);
            
            Debug.Log("删除测试物品完成");
            
            // 触发更新事件
            EventHandler.CallUpdateIngredients();
        }
    }
    
    /// <summary>
    /// 测试切换背包显示
    /// </summary>
    private void TestToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = inventoryPanel.gameObject.activeSelf;
            inventoryPanel.gameObject.SetActive(!isActive);
            
            if (!isActive)
            {
                // 如果激活背包，调用打开方法
                var panelScript = inventoryPanel.GetComponent<InventoryPanel>();
                if (panelScript != null)
                {
                    panelScript.OnInventoryOpened();
                }
            }
            
            Debug.Log($"背包{(isActive ? "关闭" : "打开")}");
        }
    }
    
    void Update()
    {
        // 键盘快捷键测试（可选）
        if (Input.GetKeyDown(KeyCode.I))
        {
            TestToggleInventory();
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            TestAddItem();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            TestRemoveItem();
        }
    }
}