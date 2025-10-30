using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingUI : MonoBehaviour
{
    public RecipeSO currentRecipe;

    [Header("UI 面板")]
    public GameObject selectionPanel;
    public GameObject cookingPanel;
    public GameObject scoringPanel;

    [Header("选材界面元素")]
    public Image dragIcon;
    public Image mainIngredientImage;
    public List<Image> garnishImages;
    public Button confirmSelectionButton;
    public Button clearSelectionButton;

    [Header("背包元素")]
    public Button inventoryPageNextButton;
    public Button inventoryPagePrevButton;
    public List<CookingBackpackSlot> backpackSlots;

    [Header("烹饪界面元素")]
    public Slider heatSlider;
    public Slider timeSlider;
    public Button confirmCookingButton;
    public Button clearCookingButton;

    [Header("烹饪界面-食材显示")]
    public Image cookingMainImage;
    public List<Image> cookingGarnishImages;

    [Header("评分界面元素")]
    public TextMeshProUGUI finalScoreText;
    public Button closeScoreButton;

    [Header("Error Popup")]
    public GameObject errorPopupPanel;
    public TextMeshProUGUI errorPopupText;
    public Button errorPopupCloseButton;

    private IngredientName selectedMainIngredient = IngredientName.None;
    private List<IngredientName> selectedGarnishes = new List<IngredientName>();
    private Sprite defaultSlotSprite;
    private Sprite cookingDefaultSlotSprite;

    private int currentPage = 0;
    private const int slotsPerPage = 16;
    private int maxPage = 0;

    private Color OpaqueColor = new Color(1, 1, 1, 1);
    private Color TransparentColor = new Color(1, 1, 1, 0);

    void Start()
    {
        Time.timeScale = 1f;

        if (mainIngredientImage != null)
        {
            defaultSlotSprite = mainIngredientImage.sprite;
            mainIngredientImage.color = TransparentColor;
        }
        foreach (var img in garnishImages)
        {
            if (img != null) img.color = TransparentColor;
        }
        if (cookingMainImage != null)
        {
            cookingDefaultSlotSprite = cookingMainImage.sprite;
            cookingMainImage.color = TransparentColor;
        }
        foreach (var img in cookingGarnishImages)
        {
            if (img != null) img.color = TransparentColor;
        }

        if (dragIcon != null)
        {
            CookingBackpackSlot.iconBeingDragged = dragIcon;
            dragIcon.gameObject.SetActive(false);
        }

        confirmSelectionButton?.onClick.AddListener(OnConfirmSelectionClicked);
        clearSelectionButton?.onClick.AddListener(OnClearSelectionClicked);
        inventoryPageNextButton?.onClick.AddListener(OnInventoryPageNext);
        inventoryPagePrevButton?.onClick.AddListener(OnInventoryPagePrev);
        confirmCookingButton?.onClick.AddListener(OnConfirmCookingClicked);
        clearCookingButton?.onClick.AddListener(OnClearCookingClicked);
        closeScoreButton?.onClick.AddListener(CloseCookingUI);

        errorPopupCloseButton?.onClick.AddListener(HideErrorPopup);
        errorPopupPanel?.SetActive(false);

        SwitchToPanel(selectionPanel);
        RefreshBackpackDisplay();
    }

    void OnEnable()
    {
        currentPage = 0;
        //RefreshBackpackDisplay();
        OnClearSelectionClicked();
    }

    public void RefreshBackpackDisplay()
    {
        if (InventoryManager.Instance == null || InventoryManager.Instance.ingredientData == null)
            return;

        List<IngredientEntry> playerIngredients = InventoryManager.Instance.ingredients;

        maxPage = Mathf.CeilToInt((float)playerIngredients.Count / slotsPerPage) - 1;
        if (maxPage < 0) maxPage = 0;
        bool showPagingButtons = (maxPage > 0);
        inventoryPagePrevButton?.gameObject.SetActive(showPagingButtons);
        inventoryPageNextButton?.gameObject.SetActive(showPagingButtons);

        for (int i = 0; i < backpackSlots.Count; i++)
        {
            int ingredientIndex = (currentPage * slotsPerPage) + i;
            if (ingredientIndex < playerIngredients.Count)
            {
                IngredientEntry entry = playerIngredients[ingredientIndex];
                IngredientDetails details = InventoryManager.Instance.ingredientData.GetItemDetails(entry.name);
                if (details != null)
                {
                    backpackSlots[i].SetupSlot(details.name, details.icon, entry.amount, details.name.ToString());
                }
                else
                {
                    backpackSlots[i].ClearSlot();
                }
            }
            else
            {
                backpackSlots[i].ClearSlot();
            }
        }
    }

    public void OnOpenCookingUI()
    {
        SwitchToPanel(selectionPanel);
    }

    private void SwitchToPanel(GameObject panelToShow)
    {
        selectionPanel?.SetActive(panelToShow == selectionPanel);
        cookingPanel?.SetActive(panelToShow == cookingPanel);
        scoringPanel?.SetActive(panelToShow == scoringPanel);
    }

    public bool TryAddIngredient(IngredientName ingredient, Sprite icon)
    {
        if (ingredient == IngredientName.None) return false;

        if (selectedMainIngredient == IngredientName.None)
        {
            selectedMainIngredient = ingredient;
            mainIngredientImage.sprite = icon;
            mainIngredientImage.color = OpaqueColor;
            return true;
        }
        if (selectedGarnishes.Count >= garnishImages.Count)
        {
            ShowErrorPopup("锅满无法添加");
            return false;
        }
        if (selectedGarnishes.Contains(ingredient))
        {
            ShowErrorPopup("辅料重复无法添加");
            return false;
        }
        int index = selectedGarnishes.Count;
        selectedGarnishes.Add(ingredient);
        garnishImages[index].sprite = icon;
        garnishImages[index].color = OpaqueColor;
        return true;
    }
    private void ShowErrorPopup(string message)
    {
        if (errorPopupPanel == null || errorPopupText == null) return;

        errorPopupText.text = message;
        errorPopupPanel.SetActive(true);
    }

    private void HideErrorPopup()
    {
        if (errorPopupPanel == null) return;

        errorPopupPanel.SetActive(false);
    }
    #region 选材界面按钮

    private void OnClearSelectionClicked()
    {
        if (InventoryManager.Instance != null)
        {
            if (selectedMainIngredient != IngredientName.None)
            {
                InventoryManager.Instance.AddIngredient(selectedMainIngredient, 1);
            }
            foreach (var garnish in selectedGarnishes)
            {
                if (garnish != IngredientName.None)
                {
                    InventoryManager.Instance.AddIngredient(garnish, 1);
                }
            }
        }
        selectedMainIngredient = IngredientName.None;
        selectedGarnishes.Clear();
        mainIngredientImage.sprite = defaultSlotSprite;
        mainIngredientImage.color = TransparentColor;
        foreach (var img in garnishImages)
        {
            img.sprite = defaultSlotSprite;
            img.color = TransparentColor;
        }
        if (cookingMainImage != null)
        {
            cookingMainImage.sprite = cookingDefaultSlotSprite;
            cookingMainImage.color = TransparentColor;
        }
        foreach (var img in cookingGarnishImages)
        {
            if (img != null)
            {
                img.sprite = cookingDefaultSlotSprite;
                img.color = TransparentColor;
            }
        }

        RefreshBackpackDisplay();
    }

    private void OnConfirmSelectionClicked()
    {
        
        if (selectedMainIngredient == IngredientName.None || selectedGarnishes.Count == 0)
        {
            ShowErrorPopup("请至少添加一种主料和一种辅料");
            return;
        }
        
        if (cookingMainImage != null)
        {
            cookingMainImage.sprite = mainIngredientImage.sprite;
            cookingMainImage.color = OpaqueColor;
        }
        for (int i = 0; i < cookingGarnishImages.Count; i++)
        {
            if (i < garnishImages.Count && garnishImages[i].sprite != defaultSlotSprite)
            {
                cookingGarnishImages[i].sprite = garnishImages[i].sprite;
                cookingGarnishImages[i].color = OpaqueColor;
            }
            else
            {
                cookingGarnishImages[i].sprite = cookingDefaultSlotSprite;
                cookingGarnishImages[i].color = TransparentColor;
            }
        }
        OnClearCookingClicked();
        SwitchToPanel(cookingPanel);
    }

    private void OnInventoryPageNext()
    {
        currentPage = (currentPage + 1) % (maxPage + 1);
        RefreshBackpackDisplay();
    }

    private void OnInventoryPagePrev()
    {
        currentPage--;
        if (currentPage < 0)
        {
            currentPage = maxPage;
        }
        RefreshBackpackDisplay();
    }

    #endregion

    #region 烹饪界面按钮

    private void OnClearCookingClicked()
    {
        if (heatSlider != null) heatSlider.value = 0.5f;
        if (timeSlider != null) timeSlider.value = 0.5f;
    }

    private void OnConfirmCookingClicked()
    {
        float playerHeat = heatSlider.value * 100f;
        float playerTime = timeSlider.value * 100f;
        if (currentRecipe == null)
        {
            Debug.LogError("没有设置当前食谱(currentRecipe)");
            return;
        }
        CookingResult result = CookingManager.Instance.CalculateScore(
            currentRecipe,
            selectedMainIngredient,
            selectedGarnishes,
            playerHeat,
            playerTime
        );
        if (finalScoreText != null)
            finalScoreText.text = $"总分: {result.finalScore}";
        
    }

    #endregion

    #region 评分界面按钮

    private void CloseCookingUI()
    {
        OnClearSelectionClicked();
        SwitchToPanel(selectionPanel);
    }

    #endregion
}