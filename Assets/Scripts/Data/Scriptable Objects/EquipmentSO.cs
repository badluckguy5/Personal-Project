using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public string equipmentName;
    public EquipmentType equipmentType;
    public Sprite icon;
    public string description;

    public StatUpgradeSO statUpgrade;

    public EquipmentAbility ability;

    public void Apply(PlayerController player)
    {
        player.GetComponent<PlayerStats>().ApplyUpgrade(statUpgrade);

        ability?.Activate(player);
    }

    public void Remove(PlayerController player)
    {
        player.GetComponent<PlayerStats>().RemoveUpgrade(statUpgrade);

        ability?.Deactivate(player);
    }
}

