using UnityEngine;

public abstract class EquipmentAbility : ScriptableObject
{
    public string abilityName;

    public abstract void Activate(PlayerController player);
    public abstract void Deactivate(PlayerController player);
}
