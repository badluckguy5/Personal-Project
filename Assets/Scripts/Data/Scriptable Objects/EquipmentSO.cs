using UnityEngine;
/*
[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/Equipment")]
public class EquipmentSO : ScriptableObject
{
    public string equipmentName;
    public EquipmentType equipmentType;
    public Sprite icon;
    public string description;

    public float speedBoost;
    public float cooldownReduction;

    public EquipmentAbility ability;

    public void Apply(PlayerController player)
    {
        player.speed += speedBoost;
        player.shootCooldown -= cooldownReduction;

        ability?.Activate(player);
    }

    public void Remove(PlayerController player)
    {
        player.speed -= speedBoost;
        player.shootCooldown += cooldownReduction;

        ability?.Deactivate(player);
    }
}

*/