using TMPro; // ！！ 导入 TextMeshPro
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CookingBackpackSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static CookingBackpackSlot slotBeingDragged;
    public static IngredientName ingredientBeingDragged;
    public static Sprite spriteBeingDragged;
    public static Image iconBeingDragged;
    public static bool dropWasSuccessful;

    public IngredientName currentIngredient { get; private set; } = IngredientName.None;
    private int slotQuantity;
    private CookingUI cookingUI;
    private string currentTooltipText = "";

    public Image ingredientIcon;
    public TextMeshProUGUI quantityText;

    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        cookingUI = GetComponentInParent<CookingUI>();
        if (cookingUI == null)
            Debug.LogError($"CookingBackpackSlot: 找不到父级{nameof(CookingUI)}脚本");
        if (ingredientIcon == null)
            Debug.LogError($"CookingBackpackSlot:{gameObject.name}没指定ingredientIcon");
        if (quantityText == null)
            Debug.LogError($"CookingBackpackSlot:{gameObject.name}没指定quantityText");

        ClearSlot();
    }

    public void SetupSlot(IngredientName ingredient, Sprite icon, int quantity, string tooltipDisplayString)
    {
        currentIngredient = ingredient;
        slotQuantity = quantity;
        currentTooltipText = tooltipDisplayString;

        ingredientIcon.sprite = icon;
        ingredientIcon.enabled = true;

        if (quantity > 1)
        {
            quantityText.text = "x" + quantity;
            quantityText.enabled = true;
        }
        else
        {
            quantityText.text = "";
            quantityText.enabled = false;
        }
    }


    public void ClearSlot()
    {
        currentIngredient = IngredientName.None;
        slotQuantity = 0;
        currentTooltipText = "";

        ingredientIcon.sprite = null;
        ingredientIcon.enabled = false;

        quantityText.text = "";
        quantityText.enabled = false;

        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentIngredient == IngredientName.None || slotQuantity <= 0 || iconBeingDragged == null || cookingUI == null)
        {
            return;
        }

        slotBeingDragged = this;
        ingredientBeingDragged = currentIngredient;
        spriteBeingDragged = ingredientIcon.sprite;
        dropWasSuccessful = false;

        //InventoryManager.Instance.ConsumeIngredient(currentIngredient, 1);
        //cookingUI.RefreshBackpackDisplay();

        iconBeingDragged.gameObject.SetActive(true);
        iconBeingDragged.sprite = spriteBeingDragged;
        iconBeingDragged.SetNativeSize();
        iconBeingDragged.rectTransform.position = Input.mousePosition;

        ingredientIcon.enabled = false;
        quantityText.enabled = false;
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconBeingDragged != null && iconBeingDragged.gameObject.activeInHierarchy)
        {
            iconBeingDragged.rectTransform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slotBeingDragged != this) return;

        if (!dropWasSuccessful)
        {
            ingredientIcon.enabled = true;
            if (slotQuantity > 1)
            {
                quantityText.enabled = true;
            }
        }
        slotBeingDragged = null;
        ingredientBeingDragged = IngredientName.None;
        spriteBeingDragged = null;
        if (iconBeingDragged != null)
        {
            iconBeingDragged.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipObject == null || tooltipText == null) return;
        if (currentIngredient == IngredientName.None) return;

        tooltipText.text = currentTooltipText;
        tooltipObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }
}