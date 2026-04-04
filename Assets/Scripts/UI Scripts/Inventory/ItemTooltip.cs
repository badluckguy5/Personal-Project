using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemTooltip : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")]
    [SerializeField] private float fadeSpeed = 10f;
    [SerializeField] private Vector2 offset = new Vector2(20f, -20f);

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
}
