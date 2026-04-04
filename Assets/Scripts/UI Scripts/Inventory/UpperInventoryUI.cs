using System.Collections.Generic;
using UnityEngine;

public class UpperInventoryUI : MonoBehaviour
{
    public Transform headgears;
    public Transform chestpieces;
    public Transform gloves;

    public GameObject slotPrefab;

    private List<ItemUISlot> headSlots = new List<ItemUISlot>();
    private List<ItemUISlot> chestSlots = new List<ItemUISlot>();
    private List<ItemUISlot> gloveSlots = new List<ItemUISlot>();

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
        RefreshRow(headgears, EquipmentType.Head, headSlots);
        RefreshRow(chestpieces, EquipmentType.Chest, chestSlots);
        RefreshRow(gloves, EquipmentType.Hands, gloveSlots);
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
            if (i < items.Count)
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
