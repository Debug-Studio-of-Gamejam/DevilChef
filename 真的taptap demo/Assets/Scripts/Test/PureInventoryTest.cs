using UnityEngine;

/// <summary>
/// 纯代码测试系统 - 只测试食材添加/减少逻辑，不涉及UI
/// 使用快捷键和日志进行测试，方便删除
/// </summary>
public class PureInventoryTest : MonoBehaviour
{
    [Header("测试设置")]
    public bool enableTest = true; // 是否启用测试
    public bool autoInitialize = true; // 是否自动初始化
    public bool useManualTest = false; // 是否使用手动测试（如果快捷键不好使）
    
    private void Start()
    {
        if (!enableTest) return;
        
        if (autoInitialize)
        {
            InitializeTest();
        }
        
        Debug.Log("纯代码测试系统已启动");
        Debug.Log("快捷键说明：");
        Debug.Log("A - 添加测试食材");
        Debug.Log("R - 移除测试食材");
        Debug.Log("C - 清空所有食材");
        Debug.Log("L - 列出当前食材");
        Debug.Log("F1 - 重新初始化测试");
    }
    
    /// <summary>
    /// 初始化测试环境
    /// </summary>
    private void InitializeTest()
    {
        Debug.Log("=== 初始化测试环境 ===");
        
        // 检查EventSystem，避免重复创建
        CheckEventSystem();
        
        // 确保InventoryManager存在
        if (InventoryManager.Instance == null)
        {
            GameObject managerObj = new GameObject("InventoryManager_Test");
            managerObj.AddComponent<InventoryManager>();
            Debug.Log("创建测试用InventoryManager");
        }
        
        // 清空现有数据
        ClearAllIngredients();
        
        // 添加一些初始测试数据
        AddTestIngredients();
        
        Debug.Log("测试环境初始化完成");
        ListCurrentIngredients();
    }
    
    /// <summary>
    /// 检查EventSystem，确保场景中只有一个
    /// </summary>
    private void CheckEventSystem()
    {
        var eventSystems = FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystems.Length > 1)
        {
            Debug.LogWarning($"发现{eventSystems.Length}个EventSystem，建议删除多余的");
            
            // 保留第一个，删除其他的
            for (int i = 1; i < eventSystems.Length; i++)
            {
                if (eventSystems[i].gameObject.name.Contains("Test"))
                {
                    Destroy(eventSystems[i].gameObject);
                    Debug.Log("删除测试创建的EventSystem");
                }
            }
        }
        else if (eventSystems.Length == 0)
        {
            Debug.Log("未找到EventSystem，UI交互可能无法正常工作");
        }
    }
    
    /// <summary>
    /// 添加测试食材
    /// </summary>
    private void AddTestIngredients()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddIngredient(IngredientName.香槟酒, 5);
            InventoryManager.Instance.AddIngredient(IngredientName.苹果, 3);
            InventoryManager.Instance.AddIngredient(IngredientName.香蕉, 2);
            
            Debug.Log("添加初始测试食材完成");
        }
    }
    
    /// <summary>
    /// 清空所有食材
    /// </summary>
    private void ClearAllIngredients()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ingredients.Clear();
            Debug.Log("清空所有食材");
        }
    }
    
    /// <summary>
    /// 列出当前食材
    /// </summary>
    private void ListCurrentIngredients()
    {
        if (InventoryManager.Instance != null)
        {
            Debug.Log("=== 当前食材列表 ===");
            
            if (InventoryManager.Instance.ingredients.Count == 0)
            {
                Debug.Log("背包为空");
                return;
            }
            
            foreach (var entry in InventoryManager.Instance.ingredients)
            {
                Debug.Log($"{entry.name}: {entry.amount}个");
            }
            
            Debug.Log($"总计: {InventoryManager.Instance.ingredients.Count}种食材");
        }
    }
    
    /// <summary>
    /// 测试添加随机食材
    /// </summary>
    private void TestAddRandomIngredient()
    {
        if (InventoryManager.Instance != null)
        {
            // 随机选择一个食材
            IngredientName[] allIngredients = { 
                IngredientName.香槟酒, IngredientName.苹果, IngredientName.香蕉, 
                IngredientName.胡萝卜, IngredientName.番茄, IngredientName.土豆 
            };
            
            IngredientName randomIngredient = allIngredients[Random.Range(0, allIngredients.Length)];
            int randomAmount = Random.Range(1, 5);
            
            InventoryManager.Instance.AddIngredient(randomIngredient, randomAmount);
            
            Debug.Log($"添加食材: {randomIngredient} x{randomAmount}");
            ListCurrentIngredients();
        }
    }
    
    /// <summary>
    /// 测试移除随机食材
    /// </summary>
    private void TestRemoveRandomIngredient()
    {
        if (InventoryManager.Instance != null && InventoryManager.Instance.ingredients.Count > 0)
        {
            // 随机选择一个现有的食材
            int randomIndex = Random.Range(0, InventoryManager.Instance.ingredients.Count);
            var ingredientToRemove = InventoryManager.Instance.ingredients[randomIndex];
            int removeAmount = Random.Range(1, Mathf.Min(3, ingredientToRemove.amount));
            
            InventoryManager.Instance.RemoveIngredient(ingredientToRemove.name, removeAmount);
            
            Debug.Log($"移除食材: {ingredientToRemove.name} x{removeAmount}");
            ListCurrentIngredients();
        }
        else
        {
            Debug.Log("没有食材可移除");
        }
    }
    
    /// <summary>
    /// 测试特定食材操作
    /// </summary>
    private void TestSpecificIngredient()
    {
        if (InventoryManager.Instance != null)
        {
            // 测试香槟酒的添加和移除
            InventoryManager.Instance.AddIngredient(IngredientName.香槟酒, 1);
            Debug.Log("添加1个香槟酒");
            
            // 如果有香槟酒，尝试移除
            var champagne = InventoryManager.Instance.ingredients.Find(e => e.name == IngredientName.香槟酒);
            if (champagne.name != IngredientName.None && champagne.amount > 0)
            {
                InventoryManager.Instance.RemoveIngredient(IngredientName.香槟酒, 1);
                Debug.Log("移除1个香槟酒");
            }
            
            ListCurrentIngredients();
        }
    }
    
    void Update()
    {
        if (!enableTest) return;
        
        // 添加输入检测日志
        if (Input.anyKeyDown)
        {
            Debug.Log($"检测到按键: {Input.inputString}");
        }
        
        // 快捷键测试
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A键被按下 - 添加食材");
            TestAddRandomIngredient();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R键被按下 - 移除食材");
            TestRemoveRandomIngredient();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C键被按下 - 清空食材");
            ClearAllIngredients();
            Debug.Log("已清空所有食材");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L键被按下 - 列出食材");
            ListCurrentIngredients();
        }
        
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("F1键被按下 - 重新初始化");
            InitializeTest();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S键被按下 - 测试特定食材");
            TestSpecificIngredient();
        }
        
        // 测试其他按键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("空格键被按下 - 测试输入系统");
        }
    }
    
    /// <summary>
    /// 禁用测试时清理资源
    /// </summary>
    private void OnDestroy()
    {
        if (!enableTest) return;
        
        // 清理测试创建的GameObject
        var testManager = GameObject.Find("InventoryManager_Test");
        if (testManager != null)
        {
            Destroy(testManager);
            Debug.Log("清理测试资源完成");
        }
    }
}