using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public EquipmentSO itemData;

    public ItemInstance(EquipmentSO item)
    {
        itemData = item;
    }
}
