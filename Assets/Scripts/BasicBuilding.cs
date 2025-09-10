using UnityEngine;
using System.Collections;


public class Building : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public Transform spawnPoint;    // Where the enemies will spawn (can be a random or fixed point)
    private float spawnInterval = 5f;  // How often to spawn enemies (in seconds)
    private float fortDestroyTime = 3f; //How long it takes for player to destroy forts


    private bool isPlayerInside = false;  // Whether the player is inside the area


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnEnemies()
    {
        // Infinite loop to spawn enemies at regular intervals
        while (true)
        {
            if (!isPlayerInside)
            {
                // Spawn the enemy if the player is not in the area
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            }

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Trigger method to detect when the player enters the area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInside = true;  // Player is inside the area
            StopAllCoroutines();  // Stop spawning enemies immediately

            // Start the timer for 3 seconds
            StartCoroutine(DestroySpawnerAfterTime(fortDestroyTime));
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInside = false;  // Player left the area

        }
    }

    // Coroutine to destroy the spawner after 3 seconds of the player standing inside
    private IEnumerator DestroySpawnerAfterTime(float time)
    {
        Debug.Log("Fort will be destroyed in " + time + " seconds");

        // Wait for the specified time while the player stays inside
        for (int i = 0; i < time; i++)
        {

            yield return new WaitForSeconds(1);
            Debug.Log("Fort will be destroyed in " + (time - i - 1) + " seconds"); //Timer until fort destruction

            if (!isPlayerInside)
            {
                Debug.Log("Player has exited area");
                StartCoroutine(SpawnEnemies());     //If player leaves area, restart spawner
                break;

            }
        }

        // Destroy the spawner if the player is still inside
        if (isPlayerInside)
        {
            Destroy(gameObject);  // Destroy the spawner object
        }
    }
}
