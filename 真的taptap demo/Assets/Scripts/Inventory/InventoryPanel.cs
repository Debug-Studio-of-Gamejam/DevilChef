using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// 背包面板 - 与UIManager集成的新版本
/// </summary>
public class InventoryPanel : MonoBehaviour
{
    [Header("背包UI组件")]
    public Transform ingredientGrid;
    public IngredientSlot slotPrefab;
    
    [Header("分页背景")]
    public GameObject page1Background; // 第一页背景（美术资源2）
    public GameObject page2Background; // 第二页背景（美术资源3）
    
    [Header("按钮控件")]
    public Button flipPageButton; // 翻页按钮（在两页间切换）
    public Button backButton; // 返回按钮（美术资源5+7）
    
    public int itemsPerPage = 16; // 每页显示16种食材
    
    [Header("翻页动画设置")]
    public float flipAnimationDuration = 0.5f; // 翻页动画时长
    public AnimationCurve flipAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 动画曲线

    [Header("食材详情面板")]
    public GameObject detailPanel;
    public Image ingredientIcon;
    public TextMeshProUGUI ingredientName;
    public TextMeshProUGUI ingredientDescription;
    public TextMeshProUGUI ingredientAmount;

    private List<IngredientSlot> currentSlots = new List<IngredientSlot>();
    private Queue<IngredientSlot> slotPool = new Queue<IngredientSlot>(); // 对象池
    
    // 翻页相关变量
    private int currentPage = 0;
    private List<IngredientEntry> allIngredients = new List<IngredientEntry>();
    private bool isPageAnimating = false; // 是否正在翻页动画中
    private List<IngredientSlot> currentPageSlots = new List<IngredientSlot>(); // 当前页的格子
    private List<IngredientSlot> nextPageSlots = new List<IngredientSlot>(); // 下一页的格子

    void Start()
    {
        // 背包面板在Start时可能不处于激活状态
        // 初始化将在OnInventoryOpened中执行
        
        // 注册事件
        EventHandler.UpdateIngredients += OnInventoryUpdateHandler;
        EventHandler.ItemSelectedEvent += OnIngredientSelected;
        
        // 测试模式：如果面板是激活的，直接初始化
        if (gameObject.activeInHierarchy)
        {
            OnInventoryOpened();
        }
    }

    void OnDestroy()
    {
        EventHandler.UpdateIngredients -= OnInventoryUpdateHandler;
        EventHandler.ItemSelectedEvent -= OnIngredientSelected;
    }

    /// <summary>
    /// 库存更新事件处理
    /// </summary>
    private void OnInventoryUpdateHandler()
    {
        RefreshIngredients(true); // 使用即时刷新
    }

    private void InitializeInventory()
    {
        Debug.Log("初始化背包系统...");
        
        detailPanel.SetActive(false);

        // 初始显示第一页背景，隐藏第二页
        if (page1Background != null)
            page1Background.SetActive(true);
        if (page2Background != null)
            page2Background.SetActive(false);

        // 绑定按钮事件
        if (flipPageButton != null)
        {
            flipPageButton.onClick.AddListener(FlipPage);
            Debug.Log("翻页按钮事件绑定成功");
        }
        else
        {
            Debug.LogWarning("翻页按钮未找到");
        }
            
        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("返回按钮事件绑定成功");
        }
        else
        {
            Debug.LogWarning("返回按钮未找到");
        }

        // 初始刷新食材显示（无动画）
        RefreshIngredients(true);
        
        Debug.Log("背包系统初始化完成");
    }

    /// <summary>
    /// 刷新食材显示（两页系统）
    /// </summary>
    private void RefreshIngredients(bool instant = false)
    {
        // 如果正在动画中，直接返回
        if (isPageAnimating && !instant) return;
        
        // 检查InventoryManager是否存在
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager实例不存在");
            return;
        }
        
        // 获取所有食材
        allIngredients = new List<IngredientEntry>(InventoryManager.Instance.ingredients);
        
        // 两页系统，确保当前页为0或1
        if (currentPage > 1) currentPage = 1;
        if (currentPage < 0) currentPage = 0;

        // 如果是即时刷新（无动画），直接显示当前页
        if (instant)
        {
            ClearAllSlots();
            DisplayCurrentPage();
        }
        
        // 更新翻页按钮状态
        UpdatePageNavigation();
    }

    /// <summary>
    /// 从对象池获取格子
    /// </summary>
    private IngredientSlot GetSlotFromPool()
    {
        if (slotPool.Count > 0)
        {
            var slot = slotPool.Dequeue();
            slot.gameObject.SetActive(true);
            return slot;
        }
        else
        {
            return Instantiate(slotPrefab, ingredientGrid);
        }
    }

    /// <summary>
    /// 将格子返回到对象池
    /// </summary>
    private void ReturnSlotToPool(IngredientSlot slot)
    {
        if (slot != null)
        {
            slot.gameObject.SetActive(false);
            slotPool.Enqueue(slot);
        }
    }

    /// <summary>
    /// 清空所有格子
    /// </summary>
    private void ClearAllSlots()
    {
        foreach (var slot in currentSlots)
        {
            if (slot != null)
                ReturnSlotToPool(slot);
        }
        currentSlots.Clear();
        currentPageSlots.Clear();
        nextPageSlots.Clear();
    }

    /// <summary>
    /// 显示当前页的食材（无动画）
    /// </summary>
    private void DisplayCurrentPage()
    {
        if (slotPrefab == null || ingredientGrid == null || InventoryManager.Instance == null)
        {
            Debug.LogWarning("显示当前页失败：缺少必要组件");
            return;
        }
        
        int startIndex = currentPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, allIngredients.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            var entry = allIngredients[i];
            
            // 错误处理：检查食材数据是否有效
            // IngredientEntry是结构体，不能与null比较
            if (entry.name == IngredientName.None)
            {
                Debug.LogWarning($"跳过无效食材条目，索引: {i}");
                continue;
            }
            
            var details = InventoryManager.Instance.ingredientData.GetItemDetails(entry.name);
            if (details != null)
            {
                var slot = GetSlotFromPool();
                slot.SetIngredient(entry, details.icon);
                currentSlots.Add(slot);
                currentPageSlots.Add(slot);
            }
            else
            {
                Debug.LogWarning($"食材数据缺失: {entry.name}");
            }
        }
        
        Debug.Log($"显示第{currentPage + 1}页，共显示{currentPageSlots.Count}个食材");
    }

    /// <summary>
    /// 更新翻页导航（两页系统）
    /// </summary>
    private void UpdatePageNavigation()
    {
        // 两页系统，翻页按钮始终可用（除了动画期间）
        if (flipPageButton != null)
        {
            flipPageButton.interactable = !isPageAnimating;
        }
    }

    /// <summary>
    /// 显示食材详情
    /// </summary>
    private void ShowIngredientDetail(IngredientEntry entry)
    {
        var details = InventoryManager.Instance.ingredientData.GetItemDetails(entry.name);
        if (details == null) return;
        
        if (ingredientIcon != null) ingredientIcon.sprite = details.icon;
        if (ingredientName != null) ingredientName.text = entry.name.ToString();
        if (ingredientDescription != null) ingredientDescription.text = details.description;
        if (ingredientAmount != null) ingredientAmount.text = $"数量: {entry.amount}";
        
        detailPanel.SetActive(true);
    }

    /// <summary>
    /// 隐藏详情面板
    /// </summary>
    private void HideDetailPanel()
    {
        detailPanel.SetActive(false);
    }

    /// <summary>
    /// 食材选中事件处理
    /// </summary>
    private void OnIngredientSelected(ItemDetails item, bool selected)
    {
        if (selected && item != null)
        {
            // 查找对应的食材 - 直接使用食材名称匹配
            var entry = InventoryManager.Instance.ingredients.Find(e => 
                e.name.ToString() == item.name.ToString());
            if (entry.name != IngredientName.None)
            {
                ShowIngredientDetail(entry);
            }
        }
        else
        {
            HideDetailPanel();
        }
    }

    /// <summary>
    /// 翻页按钮点击事件（在两页间切换）
    /// </summary>
    private void FlipPage()
    {
        if (isPageAnimating) 
        {
            Debug.Log("正在翻页动画中，忽略点击");
            return;
        }
        
        // 在两页间切换：0->1 或 1->0
        int targetPage = currentPage == 0 ? 1 : 0;
        Debug.Log($"翻页：第{currentPage + 1}页 -> 第{targetPage + 1}页");
        StartCoroutine(FlipPageAnimation(targetPage));
    }

    /// <summary>
    /// 翻页动画效果 - 真正的"翻上来"动画
    /// </summary>
    private System.Collections.IEnumerator FlipPageAnimation(int targetPage)
    {
        isPageAnimating = true;
        
        // 获取当前页和目标页的背景
        GameObject currentBackground = currentPage == 0 ? page1Background : page2Background;
        GameObject targetBackground = targetPage == 0 ? page1Background : page2Background;
        
        if (currentBackground == null || targetBackground == null) yield break;
        
        // 显示目标页，隐藏当前页
        targetBackground.SetActive(true);
        
        // 添加CanvasGroup组件用于透明度动画
        CanvasGroup currentCanvas = currentBackground.GetComponent<CanvasGroup>();
        if (currentCanvas == null) currentCanvas = currentBackground.AddComponent<CanvasGroup>();
        
        CanvasGroup targetCanvas = targetBackground.GetComponent<CanvasGroup>();
        if (targetCanvas == null) targetCanvas = targetBackground.AddComponent<CanvasGroup>();
        
        // 设置初始状态
        currentCanvas.alpha = 1f;
        targetCanvas.alpha = 0f;
        
        // 执行淡入淡出动画
        float elapsedTime = 0f;
        while (elapsedTime < flipAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = flipAnimationCurve.Evaluate(elapsedTime / flipAnimationDuration);
            
            // 当前页淡出，目标页淡入
            currentCanvas.alpha = 1f - t;
            targetCanvas.alpha = t;
            
            yield return null;
        }
        
        // 动画完成，更新当前页
        currentPage = targetPage;
        
        // 隐藏之前的背景，重置透明度
        currentBackground.SetActive(false);
        currentCanvas.alpha = 1f;
        targetCanvas.alpha = 1f;
        
        // 刷新食材显示
        RefreshIngredients(true);
        
        // 更新按钮状态
        UpdatePageNavigation();
        
        isPageAnimating = false;
    }

    /// <summary>
    /// 准备下一页的格子
    /// </summary>
    private void PrepareNextPageSlots(int targetPage)
    {
        nextPageSlots.Clear();
        
        int startIndex = targetPage * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, allIngredients.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            var entry = allIngredients[i];
            var details = InventoryManager.Instance.ingredientData.GetItemDetails(entry.name);
            if (details != null)
            {
                var slot = Instantiate(slotPrefab, ingredientGrid);
                slot.SetIngredient(entry, details.icon);
                currentSlots.Add(slot);
                nextPageSlots.Add(slot);
                
                // 初始设置为不可见或透明
                var canvasGroup = slot.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                    canvasGroup = slot.gameObject.AddComponent<CanvasGroup>();
                canvasGroup.alpha = 1f;
            }
        }
    }

    /// <summary>
    /// 清理当前页格子
    /// </summary>
    private void ClearCurrentPageSlots()
    {
        foreach (var slot in currentPageSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        currentPageSlots.Clear();
    }

    private bool isInitialized = false;
    
    /// <summary>
    /// 当背包打开时调用
    /// </summary>
    public void OnInventoryOpened()
    {
        // 延迟初始化，确保UI组件已准备好
        if (!isInitialized)
        {
            InitializeInventory();
            isInitialized = true;
        }
        
        // 重置页码到第一页
        currentPage = 0;
        RefreshIngredients(true);
        
        Debug.Log("背包已打开");
    }

    /// <summary>
    /// 清理对象池（释放内存）
    /// </summary>
    private void ClearSlotPool()
    {
        Debug.Log($"清理对象池，当前池中有{slotPool.Count}个格子");
        while (slotPool.Count > 0)
        {
            var slot = slotPool.Dequeue();
            if (slot != null)
                Destroy(slot.gameObject);
        }
    }

    /// <summary>
    /// 当背包关闭时调用
    /// </summary>
    public void OnInventoryClosed()
    {
        HideDetailPanel();
        
        // 保留对象池中的格子，下次打开时可以重用
        // 只清理当前显示的格子，不清理对象池
        ClearAllSlots();
        
        Debug.Log($"背包已关闭，对象池中保留{slotPool.Count}个格子");
    }

    /// <summary>
    /// 返回按钮点击事件
    /// </summary>
    private void OnBackButtonClicked()
    {
        // 关闭背包面板
        UIManager.Instance.ToggleInventory(false);
    }
}