using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private float speed = 10f;              // Player movement speed
    public float shootCooldown = 1f;        // Time between shots
    public float shootRange = 10f;          // Range of the shooting
    public GameObject projectilePrefab;     // Projectile prefab to shoot
    public Transform shootPoint;            // The point from where the projectile will be shot
    private float lastShootTime = 0f;
    private float bulletSpeed = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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


}
