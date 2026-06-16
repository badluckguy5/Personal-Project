using UnityEngine;
using System.Collections;

public class BasicEnemyRanged : BasicEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] protected float range = 10f;
    [SerializeField] protected float aimTime = 2f;
    [SerializeField] protected bool shooting = false;
    [SerializeField] protected float targetDistance;
    [SerializeField] protected bool shootingEnabled = true;

    public Transform shootPoint;
    public GameObject projectilePrefab;
    
    protected void UpdateTargetDistance()
    {
        targetDistance = Vector3.Distance(transform.position, targetPosition);
        
        if (targetDistance < range)
        {
            if (shootingEnabled)
            {
                shooting = true;
                StartCoroutine(AimAndShoot());
            }

            else return;
            
        }
    }

    protected new void Update()
    {
        if (player != null && alerted && !shooting)
        {
            if (chasingPlayer)
            {
                ChasePlayer();
                UpdateTargetDistance();
            }

            else
            {
                GoToLastKnownLocation();
            }
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

        }

        shooting = false;

    }

    protected IEnumerator AimAndShoot()
    {
        yield return new WaitForSeconds(aimTime);

        Shoot();
    }

    public void EnableShooting()
    {
        shootingEnabled = true;
    }

    public void DisableShooting() 
    { 
        shootingEnabled = false;
    }
}
