using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    private float speed = 10f;              // Player movement speed
    private float shootCooldown = 1f;        // Time between shots
    private float shootRange = 10f;          // Range of the shooting
    private float bulletSpeed = 10f;

    public float GetSpeed => speed;
    public float GetShootCooldown => shootCooldown;
    public float GetShootRange => speed;
    public float GetBulletSpeed => speed;

    public void ApplyUpgrade(StatUpgradeSO upgrade)
    {
        switch (upgrade.stat)
        {
            case PlayerStat.Speed:
                Apply(ref speed, upgrade);
                Debug.Log("Speed upgraded to: " + speed);
                break;

            case PlayerStat.ShootCooldown:
                Apply(ref shootCooldown, upgrade);
                Debug.Log("Cooldown is now: " + shootCooldown);
                break;

            case PlayerStat.ShootRange:
                Apply(ref shootRange, upgrade);
                Debug.Log("Range is now: " + shootRange);
                break;

            case PlayerStat.BulletSpeed:
                Apply(ref bulletSpeed, upgrade);
                Debug.Log("Bullet speed is now: " + bulletSpeed);
                break;
        }
    }

    private void Apply(ref float stat, StatUpgradeSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Additive:
                stat += upgrade.amount;
                break;

            case UpgradeType.Multiplicative:
                stat *= upgrade.amount;
                break;
        }

        // Optional: clamp or limit upgrades
    }
}
