using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WaterZone : MonoBehaviour
{
    [Header("Water Effects")]
    [SerializeField] private float waterSlow = 0.5f;
    [SerializeField] private bool weaponDisable = true;
    [SerializeField] private StatUpgradeSO playerDebuff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.ApplyUpgrade(playerDebuff);
            }

            PlayerShooter shooter = other.GetComponent<PlayerShooter>();

            if (shooter != null)
            {
                shooter.disablePlayerShooting();
            }
        }

        if (other.CompareTag("Enemy") && weaponDisable)
        {
            BasicEnemy basicEnemy = other.GetComponent<BasicEnemy>();

            if (basicEnemy != null)
            {
                basicEnemy.ApplySpeedDebuff(waterSlow);
            }

            BasicEnemyRanged basicEnemyRanged = other.GetComponent<BasicEnemyRanged>();

            if (basicEnemyRanged != null)
            {
                basicEnemyRanged.DisableShooting();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.RemoveUpgrade(playerDebuff);
            }

            PlayerShooter shooter = other.GetComponent<PlayerShooter>();

            if (shooter != null)
            {
                shooter.enablePlayerShooting();
            }
        }

        if (other.CompareTag("Enemy") && weaponDisable)
        {
            BasicEnemy basicEnemy = other.GetComponent<BasicEnemy>();

            if (basicEnemy != null)
            {
                basicEnemy.RemoveSpeedDebuff(waterSlow);
            }

            BasicEnemyRanged basicEnemyRanged = other.GetComponent<BasicEnemyRanged>();

            if (basicEnemyRanged != null)
            {
                basicEnemyRanged.EnableShooting();
            }
        }
    }

}
