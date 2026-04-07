using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    private PlayerStats stats;
    private PlayerController controller;

    public event System.Action OnEquipmentChanged;

    private Dictionary<EquipmentType, ItemInstance> equippedItems = new Dictionary<EquipmentType, ItemInstance>();

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerController>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Equip(ItemInstance item)
    {
        EquipmentType slot = item.itemData.equipmentType;

        //Unequip old item
        if (equippedItems.ContainsKey(slot))
        {
            Unequip(slot);
        }

        equippedItems[slot] = item;
        EquipmentSO data = item.itemData;

        if (data.statUpgrade != null)
        {
            ApplyStats(data);
            Debug.Log("Equipped: " + data.equipmentName + " stats");
        }

        if (data.ability != null)
        {
            data.ability.Activate(controller);
            Debug.Log("Equipped: " + data.equipmentName + " ability");
        }

        OnEquipmentChanged?.Invoke();
        Debug.Log($"OnEquipmentChanged event fired");

    }

    public void Unequip(EquipmentType slot)
    {
        if (!equippedItems.ContainsKey(slot)) { return; }

        ItemInstance item = equippedItems[slot];
        EquipmentSO data = item.itemData;

        if (data.statUpgrade != null)
        {
            RemoveStats(data);
            Debug.Log("Unequipped: " + data.equipmentName + " stats");
        }

        if (data.ability != null)
        {
            data.ability.Deactivate(controller);
            Debug.Log("Unequiped: " + data.equipmentName + " ability");
        }

        equippedItems.Remove(slot);
        Debug.Log("Unequipped: " + data.equipmentName);

        OnEquipmentChanged?.Invoke();
    }

    public ItemInstance GetEquippedItem(EquipmentType slot)
    {
        return equippedItems.ContainsKey(slot) ? equippedItems[slot] : null;
    }

    public Dictionary<EquipmentType, ItemInstance> GetAllEquippedItems()
    {
        return equippedItems;
    }

    public bool IsItemEquipped(ItemInstance item)
    {
        if (item == null || item.itemData == null) { return false; }

        if (equippedItems.TryGetValue(item.itemData.equipmentType, out ItemInstance equipped))
        {
            return equipped == item;
        }
        return false;
    }

    private void ApplyStats(EquipmentSO item)
    {
        stats.ApplyUpgrade(item.statUpgrade);
    }

    private void RemoveStats(EquipmentSO item)
    {
        stats.RemoveUpgrade(item.statUpgrade);
    }
}
