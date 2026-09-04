using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class SaveData
{
    public int lastCompletedLevel;

    //Killcounts Dictionary
    public List<string> enemyTypes = new List<string>();
    public List<int> enemyKillCounts = new List<int>();

    //Applied Upgrades
    public List<string> appliedUpgradeIDs = new List<string>();

    //PLayer Inventory
    public List<string> playerInventoryIDs = new List<string>();

    //Player Equipped
    public List<string> playerEquippedIDs = new List<string>();

    public SaveData()
    {
        lastCompletedLevel = 0;
    }

    public void SaveKillCounts(Dictionary<EnemyType, int> killCounts) 
    {
        enemyTypes.Clear();
        enemyKillCounts.Clear();
        
        foreach (var key in killCounts)
        {
            enemyTypes.Add(key.Key.ToString());         //Convert enemyType enum to string
            enemyKillCounts.Add(key.Value);
        }
    }

    public Dictionary<EnemyType, int> LoadKillCounts()
    {
        Dictionary<EnemyType, int> killcounts = new Dictionary<EnemyType, int>();

        for (int i = 0; i < enemyTypes.Count; i++)
        {
            if (System.Enum.TryParse(enemyTypes[i], out EnemyType type))
            {
                killcounts[type] = enemyKillCounts[i];
            }
        }

        return killcounts;
    }

    public void SaveInventory(List<ItemInstance> items)
    {
        playerInventoryIDs.Clear();

        foreach (var item in items)
        {
            if (item != null && item.itemData != null && !string.IsNullOrEmpty(item.itemData.itemID))
            {
                playerInventoryIDs.Add(item.itemData.itemID);
            }
        }
    }

    public List<ItemInstance> LoadInventory(Dictionary<string, EquipmentSO> itemDictionary)
    {
        List<ItemInstance> items = new List<ItemInstance>();

        foreach (var itemID in playerInventoryIDs)
        {
            if (itemDictionary.TryGetValue(itemID, out EquipmentSO item))
            {
                items.Add(new ItemInstance(item));
            }
            else
            {
                Debug.Log($"ItemID {itemID} was not found in dictionary");
            }
        }

        return items;
    }

}

