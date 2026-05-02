using UnityEngine;
using System.Collections;

public class BasicEnemyRanged : BasicEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] protected float range = 10f;
    [SerializeField] protected float aimTime = 2f;
    [SerializeField] protected bool shooting = false;
    [SerializeField] private float targetDistance;

    public Transform shootPoint;
    public GameObject projectilePrefab;
    
    private void UpdateTargetDistance()
    {
        targetDistance = Vector3.Distance(transform.position, targetPosition);
        
        if (targetDistance < range)
        {
            shooting = true;
            StartCoroutine(AimAndShoot());
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
                UpdateTargetDistance();
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

    private IEnumerator AimAndShoot()
    {
        yield return new WaitForSeconds(aimTime);

        Shoot();
    }

}
