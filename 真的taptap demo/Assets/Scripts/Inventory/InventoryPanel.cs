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
    public ItemSlot slotPrefab;
    
    [Header("分页背景")]
    public GameObject page1Background; // 第一页背景（美术资源2）
    public GameObject page2Background; // 第二页背景（美术资源3）
    
    [Header("按钮控件")]
    public Button nextPageButton; // 翻页按钮（美术资源4+6）
    public Button prevPageButton; // 上一页按钮（如果需要双向翻页）
    public Button backButton; // 返回按钮（美术资源5+7）
    
    public int itemsPerPage = 16; // 每页显示16种食材
    
    [Header("翻页动画设置")]
    public float pageFlipDuration = 0.5f; // 翻页动画时长
    public AnimationCurve pageFlipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 动画曲线

    [Header("食材详情面板")]
    public GameObject detailPanel;
    public Image ingredientIcon;
    public TextMeshProUGUI ingredientName;
    public TextMeshProUGUI ingredientDescription;
    public TextMeshProUGUI ingredientAmount;

    private List<ItemSlot> currentSlots = new List<ItemSlot>();
    
    // 翻页相关变量
    private int currentPage = 0;
    private List<IngredientEntry> allIngredients = new List<IngredientEntry>();
    private bool isPageAnimating = false; // 是否正在翻页动画中
    private List<ItemSlot> currentPageSlots = new List<ItemSlot>(); // 当前页的格子
    private List<ItemSlot> nextPageSlots = new List<ItemSlot>(); // 下一页的格子

    void Start()
    {
        InitializeInventory();
        
        // 注册事件
        EventHandler.UpdateIngredients += OnInventoryUpdateHandler;
        EventHandler.ItemSelectedEvent += OnIngredientSelected;
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
        detailPanel.SetActive(false);

        // 初始显示第一页背景，隐藏第二页
        if (page1Background != null)
            page1Background.SetActive(true);
        if (page2Background != null)
            page2Background.SetActive(false);

        // 绑定按钮事件
        if (nextPageButton != null)
            nextPageButton.onClick.AddListener(GoToNextPage);
            
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

        // 初始刷新食材显示（无动画）
        RefreshIngredients(true);
    }

    /// <summary>
    /// 刷新食材显示（带翻页功能）
    /// </summary>
    private void RefreshIngredients(bool instant = false)
    {
        // 如果正在动画中，直接返回
        if (isPageAnimating && !instant) return;
        
        // 获取所有食材
        allIngredients = new List<IngredientEntry>(InventoryManager.Instance.ingredients);
        
        // 计算总页数
        int totalPages = Mathf.CeilToInt((float)allIngredients.Count / itemsPerPage);
        
        // 确保当前页在有效范围内
        if (currentPage >= totalPages && totalPages > 0)
            currentPage = totalPages - 1;
        if (currentPage < 0)
            currentPage = 0;

        // 如果是即时刷新（无动画），直接显示当前页
        if (instant)
        {
            ClearAllSlots();
            DisplayCurrentPage();
        }
        
        // 更新翻页按钮状态和页码显示
        UpdatePageNavigation(totalPages);
    }

    /// <summary>
    /// 清空所有格子
    /// </summary>
    private void ClearAllSlots()
    {
        foreach (var slot in currentSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
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
        int startIndex = currentPage * itemsPerPage;
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
                currentPageSlots.Add(slot);
            }
        }
    }

    /// <summary>
    /// 更新翻页导航
    /// </summary>
    private void UpdatePageNavigation(int totalPages)
    {
        // 根据当前页码切换背景显示
        if (page1Background != null && page2Background != null)
        {
            page1Background.SetActive(currentPage == 0);
            page2Background.SetActive(currentPage == 1);
        }
        
        // 更新翻页按钮状态（如果是双向翻页按钮）
        if (nextPageButton != null)
        {
            // 如果是单页翻页按钮，在最后一页时隐藏或禁用
            nextPageButton.interactable = (currentPage < totalPages - 1 && totalPages > 0);
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
            // 查找对应的食材
            var ingredientName = (IngredientName)System.Enum.Parse(typeof(IngredientName), item.name.ToString());
            var entry = InventoryManager.Instance.ingredients.Find(e => e.name == ingredientName);
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
    /// 切换到下一页（公开方法，可通过手势或快捷键调用）
    /// </summary>
    public void GoToNextPage()
    {
        if (isPageAnimating) return;
        
        int totalPages = Mathf.CeilToInt((float)allIngredients.Count / itemsPerPage);
        if (currentPage < totalPages - 1)
        {
            StartCoroutine(PageFlipAnimation(currentPage + 1, true));
        }
    }

    /// <summary>
    /// 切换到上一页（公开方法，可通过手势或快捷键调用）
    /// </summary>
    public void GoToPrevPage()
    {
        if (isPageAnimating) return;
        
        if (currentPage > 0)
        {
            StartCoroutine(PageFlipAnimation(currentPage - 1, false));
        }
    }

    /// <summary>
    /// 翻页动画效果 - 物品上下交换
    /// </summary>
    private System.Collections.IEnumerator PageFlipAnimation(int targetPage, bool isNextPage)
    {
        isPageAnimating = true;
        
        // 准备下一页的格子
        PrepareNextPageSlots(targetPage);
        
        // 设置初始位置
        Vector2 currentPageStartPos = Vector2.zero;
        Vector2 nextPageStartPos = isNextPage ? new Vector2(0, 600) : new Vector2(0, -600);
        Vector2 currentPageEndPos = isNextPage ? new Vector2(0, -600) : new Vector2(0, 600);
        Vector2 nextPageEndPos = Vector2.zero;
        
        // 设置初始位置
        foreach (var slot in currentPageSlots)
        {
            if (slot != null)
            {
                var rect = slot.GetComponent<RectTransform>();
                rect.anchoredPosition = currentPageStartPos;
            }
        }
        
        foreach (var slot in nextPageSlots)
        {
            if (slot != null)
            {
                var rect = slot.GetComponent<RectTransform>();
                rect.anchoredPosition = nextPageStartPos;
            }
        }
        
        // 执行动画
        float elapsedTime = 0f;
        while (elapsedTime < pageFlipDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = pageFlipCurve.Evaluate(elapsedTime / pageFlipDuration);
            
            // 当前页向上/下移动
            foreach (var slot in currentPageSlots)
            {
                if (slot != null)
                {
                    var rect = slot.GetComponent<RectTransform>();
                    rect.anchoredPosition = Vector2.Lerp(currentPageStartPos, currentPageEndPos, t);
                }
            }
            
            // 下一页向下/上移动
            foreach (var slot in nextPageSlots)
            {
                if (slot != null)
                {
                    var rect = slot.GetComponent<RectTransform>();
                    rect.anchoredPosition = Vector2.Lerp(nextPageStartPos, nextPageEndPos, t);
                }
            }
            
            yield return null;
        }
        
        // 动画完成，更新当前页
        currentPage = targetPage;
        
        // 清理当前页格子，设置下一页为当前页
        ClearCurrentPageSlots();
        currentPageSlots = new List<ItemSlot>(nextPageSlots);
        nextPageSlots.Clear();
        
        // 更新页码显示
        int totalPages = Mathf.CeilToInt((float)allIngredients.Count / itemsPerPage);
        UpdatePageNavigation(totalPages);
        
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

    /// <summary>
    /// 当背包打开时调用
    /// </summary>
    public void OnInventoryOpened()
    {
        // 重置页码到第一页
        currentPage = 0;
        RefreshIngredients(true);
    }

    /// <summary>
    /// 当背包关闭时调用
    /// </summary>
    public void OnInventoryClosed()
    {
        HideDetailPanel();
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