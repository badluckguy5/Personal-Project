using UnityEngine;

public class TriggerSpawn : MonoBehaviour
{
    private BasicBuilding spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = GetComponentInParent<BasicBuilding>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.SpawnOneWave();
            spawner.StartSpawning();
            Destroy(gameObject);
        }
    }
}
