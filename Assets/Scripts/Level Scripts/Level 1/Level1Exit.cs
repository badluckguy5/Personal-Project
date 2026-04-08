using UnityEngine;

public class Level1Exit : MonoBehaviour
{
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelManager != null)
        {
            levelManager.CompleteLevel();
        }
    }
}
