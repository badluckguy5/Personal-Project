using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragHandle;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private float edgePadding = 10f;

    private RectTransform windowRect;
    private bool isDragging = false;

    private void Awake()
    {
        windowRect = GetComponent<RectTransform>();

        if (dragHandle == null)
        {
            Debug.LogError("Drag handle not assigned!", this);
            enabled = false;
            return;
        }

        if (targetCanvas == null)
            targetCanvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the actual GameObject clicked is the drag handle itself
        if (eventData.pointerCurrentRaycast.gameObject == dragHandle.gameObject)
        {
            isDragging = true;
        }
        else
        {
            isDragging = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        float scale = targetCanvas.scaleFactor;
        Vector2 movement = eventData.delta / scale;
        Vector2 newPosition = windowRect.anchoredPosition + movement;

        newPosition = ClampToScreenBounds(newPosition);
        
        windowRect.anchoredPosition = newPosition;
    }

    private Vector2 ClampToScreenBounds(Vector2 position)
    {
        RectTransform canvasRect = windowRect.parent as RectTransform;
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 windowSize = windowRect.rect.size;

        float canvasScale = targetCanvas.scaleFactor;
        float padding = edgePadding / canvasScale;

        float minX = -canvasSize.x / 2f + (windowSize.x / 2f) + padding;
        float maxX = canvasSize.x / 2f - (windowSize.x / 2f) - padding;
        float minY = -canvasSize.y / 2f + (windowSize.y / 2f) + padding;
        float maxY = canvasSize.y / 2f - (windowSize.y / 2f) - padding;

        if (minX > maxX)
        {
            float centerX = (minX + maxX) / 2f;
            minX = centerX;
            maxX = centerX;
        }
        if (minY > maxY)
        {
            float centerY = (minY + maxY) / 2f;
            minY = centerY;
            maxY = centerY;
        }

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
}