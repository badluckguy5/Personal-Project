using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class UpgradeSystem
{
    private readonly Dictionary<(EnemyType, int), StatUpgradeSO> milestones;

    public UpgradeSystem()
    {
        // Dynamically load all StatUpgradeSO assets
        milestones = new Dictionary<(EnemyType, int), StatUpgradeSO>();

        var allUpgrades = Resources.LoadAll<StatUpgradeSO>("Upgrades");

        foreach (var upgrade in allUpgrades)
        {
            var key = (upgrade.appliesToEnemy, upgrade.unlockKillCount);

            if (!milestones.ContainsKey(key))
            {
                milestones[key] = upgrade;
            }
            else
            {
                Debug.LogWarning($"Duplicate upgrade milestone detected for {key}. Ignoring duplicate: {upgrade.upgradeName}");
            }
        }
    }

    public bool TryGetUpgrade(EnemyType type, int killCount, out StatUpgradeSO upgrade)
    {
        return milestones.TryGetValue((type, killCount), out upgrade);
    }

    public bool TryGetUpgradeByID(string upgradeID, out StatUpgradeSO upgrade)
    {
        string[] parts = upgradeID.Split('_');
        if (parts.Length == 2)
        {
            EnemyType type = (EnemyType)System.Enum.Parse(typeof(EnemyType), parts[0]);
            int killCount = int.Parse(parts[1]);
            return TryGetUpgrade(type, killCount, out upgrade);
        }

        upgrade = null;
        return false;
    }

}
