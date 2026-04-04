using UnityEngine;
using System.Collections.Generic;
using System;


public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ItemInstance> items = new List<ItemInstance>();

    public event Action OnInventoryChanged;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(EquipmentSO item)
    {
        items.Add(new ItemInstance(item));
        Debug.Log("Added: " + item.equipmentName);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemInstance item)
    {
        items.Remove(item);
        Debug.Log("Removed: " + item.itemData.name);
        OnInventoryChanged?.Invoke();
    }

    public List<ItemInstance> GetItemsByType(EquipmentType type) {
        return items.FindAll(i => i.itemData.equipmentType == type);
    }

}
