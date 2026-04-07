using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemTooltip : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeSpeed = 10f;
    [SerializeField] private Vector2 itemOffset = new Vector2(20f, -20f);
    [SerializeField] private Vector2 abilityOffset = new Vector2(20f, 200f);

    private Vector2 offset;

    private RectTransform rectTransform;
    private bool isVisible = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (canvasGroup != null)
        {
            float targetAlpha = isVisible ? 1f : 0f;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        }

        if (isVisible)
        {
            UpdatePosition();
        }
    }

    public void Show(ItemInstance item, Vector2 mousePosition)
    {
        if (item == null || item.itemData == null) { return; }

        //Set text data
        if (itemNameText != null)
        {
            itemNameText.text = item.itemData.name;
        }

        if (itemTypeText != null)
        {
            itemTypeText.text = item.itemData.equipmentType.ToString();
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemData.description;
        }

        isVisible = true;

        offset = itemOffset;
    }

    public void Hide()
    {
        isVisible = false;
    }

    public void UpdatePosition()
    {
        //Get mouse position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out Vector2 localPoint);

        //Apply offset
        localPoint += offset;

        //Clamp to screen edges
        RectTransform parentRect = transform.parent as RectTransform;
        Vector2 tooltipSize = rectTransform.rect.size;
        Vector2 parentSize = parentRect.rect.size;

        localPoint.x = Mathf.Clamp(localPoint.x, -parentSize.x / 2f + tooltipSize.x / 2f, parentSize.x / 2f - tooltipSize.x / 2f);
        localPoint.y = Mathf.Clamp(localPoint.y, -parentSize.y / 2f + tooltipSize.y / 2f, parentSize.y / 2f - tooltipSize.y / 2f);

        rectTransform.anchoredPosition = localPoint;
    }

    public void ShowAbility(EquipmentAbility ability, Vector2 mousePosition)
    {
        if (ability == null) return;

        if (itemNameText != null)
        {
            itemNameText.text = ability.abilityName;
        }

        if (itemTypeText != null)
        {
            itemTypeText.text = ability.GetCooldown() > 0 ? "Active Ability" : "Passive Ability";
        }

        if (itemDescriptionText != null)
        {
            string description = ability.abilityDescription;

            if (ability.GetCooldown() > 0)
            {
                description += $"\n<color=yellow>Cooldown: {ability.GetCooldown()} seconds</color>";
            }

            itemDescriptionText.text = description;
        }

        isVisible = true;
        offset = abilityOffset;
        UpdatePosition();
    }
}

