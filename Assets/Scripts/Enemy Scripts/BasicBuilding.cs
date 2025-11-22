using UnityEngine;
using System.Collections;


public class BasicBuilding : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public Transform[] spawnPoints;    // Where the enemies will spawn (can be a random or fixed point)
    public float spawnInterval = 5f;  // How often to spawn enemies (in seconds)
    public float fortDestroyTime = 3f; //How long it takes for player to destroy forts

    protected FloatingMessageSpawner gameMessages;

    private bool subscribedToEvents = false;

    protected bool isPlayerInsideZone = false;  // Whether the player is inside the area

    protected virtual void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            SubscribeToLevelManagerEvents();
        }
        else
        {
            StartCoroutine(WaitForLevelManagerAndSubscribe());
        }
    }

    protected virtual void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerWentIndoors -= SetSlowSpawn;
            LevelManager.Instance.OnPlayerWentOutdoors -= SetNormalSpawn;
        }
    }

    private System.Collections.IEnumerator WaitForLevelManagerAndSubscribe()
    {
        while (LevelManager.Instance == null)
        {
            yield return null; // wait for next frame
        }
        SubscribeToLevelManagerEvents();
    }

    private void SubscribeToLevelManagerEvents()
    {
        if (!subscribedToEvents)
        {
            LevelManager.Instance.OnPlayerWentIndoors += SetSlowSpawn;
            LevelManager.Instance.OnPlayerWentOutdoors += SetNormalSpawn;
            subscribedToEvents = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        GameObject messageManagerObject = GameObject.Find("Message Manager");

        if ( messageManagerObject != null)
        {
            gameMessages = messageManagerObject.GetComponent<FloatingMessageSpawner>();
        }
        else
        {
            Debug.LogError("Message Manager GameObject not found!");
        }

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual IEnumerator SpawnEnemies()
    {
        // Infinite loop to spawn enemies at regular intervals
        while (true)
        {
            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);

            if (!isPlayerInsideZone)
            {
                SpawnOneWave();        
            }        
        }
    }

    // Trigger method to detect when the player enters the area
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInsideZone = true;  // Player is inside the area
            StopAllCoroutines();  // Stop spawning enemies immediately

            // Start the timer for 3 seconds
            StartCoroutine(DestroySpawnerAfterTime(fortDestroyTime));
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))  // Check if the object is the player
        {
            isPlayerInsideZone = false;  // Player left the area

        }
    }

    // Coroutine to destroy the spawner after 3 seconds of the player standing inside
    protected virtual IEnumerator DestroySpawnerAfterTime(float time)
    {
        gameMessages.ShowMessage("Fort will be destroyed in " + time + " seconds");

        // Wait for the specified time while the player stays inside
        for (int i = 0; i < time; i++)
        {

            yield return new WaitForSeconds(1);
            gameMessages.ShowMessage("Fort will be destroyed in " + (time - i - 1) + " seconds"); //Timer until fort destruction

            if (!isPlayerInsideZone)
            {
                gameMessages.ShowMessage("Player has exited area");
                StartCoroutine(SpawnEnemies());     //If player leaves area, restart spawner
                break;

            }
        }

        // Destroy the spawner if the player is still inside
        if (isPlayerInsideZone)
        {
            Destroy(gameObject);  // Destroy the spawner object
        }
    }

    public void SpawnOneWave()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {

            // Spawn the enemy if the player is not in the area and get a reference to the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);

            //Try to get the BasicEnemy component and set it as alerted
            BasicEnemy basicEnemy = enemy.GetComponent<BasicEnemy>();
            if (basicEnemy != null)
            {
                basicEnemy.setAlerted();
            }

        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void SetSlowSpawn(Vector3 _)
    {
        spawnInterval = spawnInterval * 3;
    }

    private void SetNormalSpawn()
    {
        spawnInterval = spawnInterval / 3;
    }
}
