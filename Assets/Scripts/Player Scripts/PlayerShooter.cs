using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerStats stats;
    private EnemyFinder finder;

    [Header("Projectile")]
    public GameObject projectilePrefab;     // Projectile prefab to shoot
    public Transform shootPoint;            // The point from where the projectile will be shot

    private float lastShootTime = 0f;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        finder = GetComponent<EnemyFinder>();
    }

    private bool CanShoot()
        => Time.time >= lastShootTime + stats.GetShootCooldown;

    public void PlayerShoot()
    {
        if (!CanShoot()) { return; }

        GameObject closestEnemy = finder.FindClosestEnemy(stats.GetShootRange);

        if (closestEnemy == null) { return; }

        // Calculate direction to the enemy
        Vector3 direction = (closestEnemy.transform.position - shootPoint.position).normalized;

        // Fire a projectile towards the enemy
        FireProjectile(direction);

        lastShootTime = Time.time;
    }

    void FireProjectile(Vector3 direction)
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * stats.GetBulletSpeed;
            }
        }

    }
}
