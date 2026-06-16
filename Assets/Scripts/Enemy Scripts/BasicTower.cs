using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BasicTower : Creature
{
    [Header("Tower Stats")]
    [SerializeField] protected float shootCooldown;
    [SerializeField] protected float aimTime;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] private float lastShootTime;
    [SerializeField] private bool isAiming = false;
    [SerializeField] protected bool playerInRange = false;

    protected GameObject player;
    protected bool canShoot = true;

    protected override void Start()
    {
        base.Start();

        player = GameObject.Find("Player");
    }
    
    private void Update()
    {
        if (!playerInRange || player == null) return;


        if (Time.time >= lastShootTime + shootCooldown && !isAiming && canShoot)
        {
            StartCoroutine(AimAndShoot());
        }
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    protected IEnumerator AimAndShoot()
    {
        isAiming = true;

        yield return new WaitForSeconds(aimTime);

        if (player != null)
        {
            Shoot();
            //lastShootTime = Time.time;

        }

    }

    protected void Shoot()
    {
        if (player != null && shootPoint != null)
        {
            Vector3 playerDirection = (player.transform.position - shootPoint.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

            BasicEnemyProjectile projScript = projectile.GetComponent<BasicEnemyProjectile>();
            if (projScript != null)
            {
                projScript.Initizalize(playerDirection);
            }

            isAiming = false;
            lastShootTime = Time.time;
        }
    }
}
