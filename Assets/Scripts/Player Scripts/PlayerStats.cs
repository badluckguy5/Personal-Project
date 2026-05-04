using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField]private float speed = 10f;              // Player movement speed
    [SerializeField] private float shootCooldown = 1f;        // Time between shots
    [SerializeField] private float shootRange = 10f;          // Range of the shooting
    [SerializeField] private float bulletSpeed = 10f;

    public float GetSpeed => speed;
    public float GetShootCooldown => shootCooldown;
    public float GetShootRange => shootRange;
    public float GetBulletSpeed => bulletSpeed;

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

    public void RemoveUpgrade(StatUpgradeSO upgrade)
    {
        switch (upgrade.stat)
        {
            case PlayerStat.Speed:
                Remove(ref speed, upgrade);
                Debug.Log("Speed reverted to: " + speed);
                break;

            case PlayerStat.ShootCooldown:
                Remove(ref shootCooldown, upgrade);
                Debug.Log("Cooldown reverted to: " + shootCooldown);
                break;

            case PlayerStat.ShootRange:
                Remove(ref shootRange, upgrade);
                Debug.Log("Range reverted to: " + shootRange);
                break;

            case PlayerStat.BulletSpeed:
                Remove(ref bulletSpeed, upgrade);
                Debug.Log("Bullet speed reverted to: " + bulletSpeed);
                break;
        }
    }

    private void Remove(ref float stat, StatUpgradeSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Additive:
                stat -= upgrade.amount;
                break;

            case UpgradeType.Multiplicative:
                stat /= upgrade.amount;
                break;
        }

        // Optional: clamp back to minimums
    }

}
