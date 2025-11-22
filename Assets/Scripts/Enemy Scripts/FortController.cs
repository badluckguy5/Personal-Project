using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class FortController : BasicBuilding
{

    private float playerInvadeTime = 3f;
    public float reinforcementTime = 4.5f;
    private UnityEngine.Vector3 playerLastLocation;
    public Transform indoorsLocation;
    public Transform exitLocation;
    public Transform enemyIndoorsTeleport;
    public BoxCollider triggerCollider;

    private bool playerInsideFort = false;

    private Queue<BasicEnemy> enemyQueue = new Queue<BasicEnemy>();
    private bool enemyTeleportRoutineRunning = false;
    private Transform enemiesContainer;

    protected override void Start()
    {
        base.Start();

        enemiesContainer = transform.Find("Fort Interior/Enemies");
    }

    // Trigger method to detect when the player enters the area
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInsideZone = true;  // Player is inside the area
            StopAllCoroutines();  // Stop spawning enemies immediately

            // Start the timer for 3 seconds
            StartCoroutine(TeleportPlayerAfterTime(playerInvadeTime));

        }

        if (other.CompareTag("Enemy"))
        {
            if (playerInsideFort)
            {
                BasicEnemy enemy = other.GetComponent<BasicEnemy>();
                if (enemy != null)      // Add enemy to queue to teleport inside
                {
                    if (!enemyQueue.Contains(enemy)) // optional: avoid duplicates
                        enemyQueue.Enqueue(enemy);
                }

                if (!enemyTeleportRoutineRunning && enemyQueue.Count > 0)
                {
                    StartCoroutine(TeleportEnemyAfterTime(reinforcementTime));
                    enemyTeleportRoutineRunning = true;
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInsideZone = false;  // Player left the area

        }
    }

    private IEnumerator TeleportPlayerAfterTime(float time)
    {
        gameMessages.ShowMessage("Player will invade fort in " + time + " seconds");

        // Wait for the specified time while the player stays inside
        for (int i = 0; i < time; i++)
        {

            yield return new WaitForSeconds(1);
            gameMessages.ShowMessage("Player will invade fort in " + (time - i - 1) + " seconds"); //Timer until player invasion

            if (!isPlayerInsideZone)
            {
                gameMessages.ShowMessage("Player has exited area");
                StartCoroutine(SpawnEnemies());     //If player leaves area, restart spawner
                break;

            }
        }

        // Teleport player inside
        if (isPlayerInsideZone)
        {
            // Find the player's Transform
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null && indoorsLocation != null)
            {
                playerLastLocation = player.transform.position;
                LevelManager.Instance.SetPlayerLastLocation(this, playerLastLocation);
                LevelManager.Instance.SetPlayerIndoors(this, true);


                player.transform.position = indoorsLocation.position;
                player.transform.rotation = indoorsLocation.rotation;

                player.GetComponent<PlayerController>().indoors = true;

                playerInsideFort = true;

                gameMessages.ShowMessage("Player has invaded the fort!");   // Teleport player inside fort
            }

            //Set enemies in fort active
            enemiesContainer.gameObject.SetActive(true);

            // Queue any enemies already inside the fort trigger area
            AddEnemiesAlreadyInside();

            // Start enemy teleport coroutine if not already running
            if (!enemyTeleportRoutineRunning && enemyQueue.Count > 0)
            {
                StartCoroutine(TeleportEnemyAfterTime(reinforcementTime));
                enemyTeleportRoutineRunning = true;
            }

        }
    }

    public void PlayerExitFort()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<PlayerController>().indoors = false;

        player.transform.position = exitLocation.position;
        player.transform.rotation = exitLocation.rotation;

        playerInsideFort = false;
        enemyQueue.Clear();

        LevelManager.Instance.SetPlayerIndoors(this, false);


        if (!GetFortIntact())
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SpawnEnemies());     //If player leaves area and fort is intact, restart spawner

            //Set enemies in fort inactive
            enemiesContainer.gameObject.SetActive(false);

        }

    }

    private IEnumerator TeleportEnemyAfterTime(float time)
    {
        while (enemyQueue.Count > 0 && playerInsideFort)
        {
            yield return new WaitForSeconds(time);

            BasicEnemy nextEnemy = enemyQueue.Dequeue();

            if (nextEnemy != null && nextEnemy.gameObject != null)
            {
                nextEnemy.transform.position = enemyIndoorsTeleport.position;
                nextEnemy.transform.rotation = enemyIndoorsTeleport.rotation;
                nextEnemy.SetChasePlayer();
                nextEnemy.RestoreSpeed();
                nextEnemy.indoors = true;
                

                nextEnemy.transform.SetParent(enemiesContainer, true);
            }
        }

        enemyTeleportRoutineRunning = false;
    }

    private void AddEnemiesAlreadyInside()
    {
        Bounds triggerBounds = triggerCollider.bounds;  // use the assigned trigger collider
        Collider[] hits = Physics.OverlapBox(triggerBounds.center, triggerBounds.extents);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                BasicEnemy enemy = hit.GetComponent<BasicEnemy>();
                if (enemy != null && !enemyQueue.Contains(enemy))
                    enemyQueue.Enqueue(enemy);
            }
        }
    }

    public bool GetFortIntact()
    {
        // Find the child called "Fort Cores"
        Transform fortCoresTransform = transform.Find("Fort Interior/Enemies/Fort Cores");

        if (fortCoresTransform == null)
            return false;

        // Check if there are any children under "Fort Cores"
        foreach (Transform core in fortCoresTransform)
        {
            if (core.name.StartsWith("Fort Core"))
            {
                // Found at least one Fort Core
                return true;
            }
        }

        return false;
    }
}
