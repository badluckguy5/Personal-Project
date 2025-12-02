using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    public GameObject FindClosestEnemy(float maxRange)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Assuming all enemies have "Enemy" tag
       
        GameObject closestEnemy = null;
        float bestDistance = Mathf.Infinity; // Start with an infinite distance
        Vector3 pos = transform.position;

        foreach (GameObject enemy in enemies)   //Go through all enemies
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < bestDistance && distance <= maxRange)       //Find closest enemy
            {
                bestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
