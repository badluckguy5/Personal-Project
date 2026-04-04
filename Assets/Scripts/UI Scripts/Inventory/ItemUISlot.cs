using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ItemUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private ItemTooltip tooltip;
    [SerializeField] private Image borderImage;

    private ItemInstance currentItem;

    private void Start()
    {
        if (tooltip == null)
        {
            tooltip = FindAnyObjectByType<ItemTooltip>();
        }

        if (borderImage != null)
        {
            borderImage.enabled = false;
        }
    }

    public void SetItem(ItemInstance item)
    {
        currentItem = item;

        if (item != null && item.itemData != null)
        {
            icon.sprite = item.itemData.icon;
            icon.enabled = true;
            UpdateBorder();
        }

        else
        {
            icon.sprite = null;
            icon.enabled = false;
            borderImage.enabled = false;
        }
    }

    public void UpdateBorder()
    {
        if (borderImage == null) return;

        if (currentItem == null || currentItem.itemData == null)
        {
            borderImage.enabled = false;
        }

        if (PlayerEquipment.Instance != null)
        {
            borderImage.enabled = PlayerEquipment.Instance.IsItemEquipped(currentItem);
        }
        else
        {
            borderImage.enabled = false;
        }

    }

    public void ClearSlot()
    {
        SetItem(null);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && tooltip != null)
        {
            tooltip.Show(currentItem, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null) return;

        if (eventData.clickCount == 2)
        {
            if (currentItem != null && currentItem.itemData != null && PlayerEquipment.Instance != null)
            {
                if (PlayerEquipment.Instance.IsItemEquipped(currentItem)) 
                {
                    PlayerEquipment.Instance.Unequip(currentItem.itemData.equipmentType);
                }

                else
                {
                    PlayerEquipment.Instance.Equip(currentItem);
                }
            }
        }
    }
}
