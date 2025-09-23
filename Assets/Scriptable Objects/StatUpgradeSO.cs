using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgradeSO", menuName = "Scriptable Objects/StatUpgradeSO")]
public class StatUpgradeSO : ScriptableObject
{
    public EnemyType appliesToEnemy;
    public int unlockKillCount;
    public string upgradeName;
    public PlayerStat stat;
    public UpgradeType upgradeType;
    public float amount;
    [TextArea]
    public string description;
}
