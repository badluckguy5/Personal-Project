using UnityEngine;
using TMPro;

public enum PlayerStat
{
    Speed,
    ShootCooldown,
    ShootRange,
    BulletSpeed
}

public enum UpgradeType
{
    Additive,
    Multiplicative
}


public class PlayerController : Creature
{
    public float speed = 10f;              // Player movement speed
    private float shootCooldown = 1f;        // Time between shots
    private float shootRange = 10f;          // Range of the shooting
    public GameObject projectilePrefab;     // Projectile prefab to shoot
    public Transform shootPoint;            // The point from where the projectile will be shot
    private float lastShootTime = 0f;
    private float bulletSpeed = 10f;

    public TMP_Text healthText;

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 3;
    }

    protected override void Start()
    {
        currentHealth = maxHealth;

        healthBarInstance = healthBarPrefab;

        healthText.text = "" + currentHealth;

    }

    // Update is called once per frame
    void Update()
    {

        MovePlayer();

        if (Time.time - lastShootTime >= shootCooldown)     //Bullet cooldown
        {
            ShootClosestEnemy();
            lastShootTime = Time.time; // Reset the cooldown timer
        }


    }

    void ShootClosestEnemy()
    {
        // Find all enemies in the scene
        GameObject closestEnemy = FindClosestEnemy();

        if (closestEnemy != null)
        {
            // Calculate direction to the enemy
            Vector3 direction = (closestEnemy.transform.position - transform.position).normalized;

            // Fire a projectile towards the enemy
            FireProjectile(direction);
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Assuming all enemies have "Enemy" tag
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity; // Start with an infinite distance

        foreach (GameObject enemy in enemies)   //Go through all enemies
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= shootRange)       //Find closest enemy
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void FireProjectile(Vector3 direction)
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }

    }

    //Moves player mased on keyboard input
    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        healthText.text = "" + currentHealth;
    }

    protected override void TriggerDeath()
    {
        Time.timeScale = 0f;
        GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();
    }

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
