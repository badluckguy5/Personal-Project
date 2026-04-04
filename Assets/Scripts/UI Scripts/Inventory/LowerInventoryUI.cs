using System.Collections.Generic;
using UnityEngine;

public class LowerInventoryUI : MonoBehaviour
{
    public Transform pants;
    public Transform feet;

    public GameObject slotPrefab;

    private List<ItemUISlot> pantSlots = new List<ItemUISlot>();
    private List<ItemUISlot> feetSlots = new List<ItemUISlot>();

    private void OnEnable()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged += RefreshAll;
        }

        if (PlayerEquipment.Instance != null)
        {
            PlayerEquipment.Instance.OnEquipmentChanged += RefreshAll;
        }

        RefreshAll();
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnInventoryChanged -= RefreshAll;
        }

        if (PlayerEquipment.Instance != null)
        {
            PlayerEquipment.Instance.OnEquipmentChanged -= RefreshAll;
        }
    }

    public void RefreshAll()
    {
        RefreshRow(pants, EquipmentType.Legs, pantSlots);
        RefreshRow(feet, EquipmentType.Feet, feetSlots);
    }
    
    public void RefreshRow(Transform parent, EquipmentType type, List<ItemUISlot> pool)
    {
        if (parent == null) return;

        List<ItemInstance> items = Inventory.Instance.GetItemsByType(type);

        //Create extra slots if needed
        while (pool.Count < items.Count)
        {
            GameObject slotObj = Instantiate(slotPrefab, parent);
            ItemUISlot slot = slotObj.GetComponent<ItemUISlot>();

            pool.Add(slot);
        }

        //Fill Slots
        for (int i = 0; i < pool.Count; i++)
        {
            if (i <items.Count)
            {
                pool[i].gameObject.SetActive(true);
                pool[i].SetItem(items[i]);
            }

            else
            {
                pool[i].gameObject.SetActive(false);
            }
        }
    }
}
