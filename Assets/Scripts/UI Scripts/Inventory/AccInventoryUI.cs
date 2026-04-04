using System.Collections.Generic;
using UnityEngine;

public class AccInventoryUI : MonoBehaviour
{
    public Transform necklaces;
    public Transform rings;

    public GameObject slotPrefab;

    private List<ItemUISlot> necklaceSlots = new List<ItemUISlot>();
    private List<ItemUISlot> ringSlots = new List<ItemUISlot>();

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
        RefreshRow(necklaces, EquipmentType.Necklace, necklaceSlots);
        RefreshRow(rings, EquipmentType.Ring, ringSlots);
    }

    public void RefreshRow(Transform parent, EquipmentType type, List<ItemUISlot> pool)
    {
        if (parent == null) return;

        List<ItemInstance> items = Inventory.Instance.GetItemsByType(type);

        while (pool.Count < items.Count)
        {
            GameObject slotObj = Instantiate(slotPrefab, parent);
            ItemUISlot slot = slotObj.GetComponent<ItemUISlot>();

            pool.Add(slot);
        }

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
