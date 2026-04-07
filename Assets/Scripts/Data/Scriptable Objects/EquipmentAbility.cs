using UnityEngine;
using UnityEngine.Rendering;

public abstract class EquipmentAbility : ScriptableObject
{
    [Header("Basic Info")]
    public string abilityName;
    [TextArea(3,5)]
    public string abilityDescription;

    [Header("Visuals")]
    public Sprite icon;

    private float lastUseTime = -999f;

    public abstract void Activate(PlayerController player);
    public abstract void Deactivate(PlayerController player);

    public virtual float GetCooldown() => 0f;

    public void RecordAbilityUse()
    {
        lastUseTime = Time.time;
    }

    public void ResetCooldown()
    {
        lastUseTime = -999f;
    }

    public float GetCooldownPercent()
    {
        float cooldown = GetCooldown();
        if (cooldown < 0f) return 0f;

        float timeSinceLastUse = Time.time - lastUseTime;
        if (timeSinceLastUse >= cooldown) return 0f;

        return (1f - (timeSinceLastUse/cooldown));

    }

    public float GetCooldownRemaining()
    {
        return (GetCooldown() - (Time.time - lastUseTime));
    }

    public bool IsReady()
    {
        float cooldown = GetCooldown();
        if ( cooldown < 0f) return true;

        return (Time.time - lastUseTime) >= cooldown;
    }

    public void CooldownMessage()
    {
        if (!IsReady())
        {
            FloatingMessageSpawner.Instance.ShowMessage($"{abilityName} on cooldown for {GetCooldownRemaining():F1} seconds");
            return;
        }
    }

    //In case I'm going to add ability tooltip when looking at items
    public virtual string GetTooltipText()
    {
        string tooltip = $"<b>{abilityName}</b>\n{abilityDescription}";

        if (GetCooldown() > 0f)
        {
            tooltip += $"\n<color=yellow>Cooldown: {GetCooldown()} seconds</color>";

        }
        return tooltip ;
    }
}
