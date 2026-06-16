using System.Collections.Generic;
using UnityEngine;

public class ItemSystem
{
    private readonly Dictionary<string, EquipmentSO> itemsByID;

    public ItemSystem()
    {
        itemsByID = new Dictionary<string, EquipmentSO>();

        EquipmentSO[] allItems = Resources.LoadAll<EquipmentSO>("Equipment");

        foreach (EquipmentSO item in allItems)
        {
            if (string.IsNullOrEmpty(item.itemID))
            {
                Debug.Log($"Item {item.equipmentName} has no itemID");
                continue;
            }

            if (!itemsByID.ContainsKey(item.itemID))
            {
                itemsByID[item.itemID] = item;
            }
            else
            {
                Debug.Log($"Duplicate item ID detected: {item.itemID}. Ignoring duplicate: {item.equipmentName}");
            }
        }

        Debug.Log($"Loaded {itemsByID.Count} items into ItemSystem");

    }

    public bool TryGetItemByID(string itemID, out EquipmentSO item)
    {
        return itemsByID.TryGetValue(itemID, out item);
    }

    public EquipmentSO GetItemByID(string itemID)
    {
        return itemsByID[itemID];
    }

    public Dictionary<string, EquipmentSO> GetAllItems()
    {
        return itemsByID;
    }

}