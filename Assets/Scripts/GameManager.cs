using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;

    public GameObject gameOverScreen;

    private FloatingMessageSpawner gameMessages;

    private StatUpgradeSO[] allUpgrades;

    private Dictionary<EnemyType, int> enemyKills = new Dictionary<EnemyType, int>();

    public Dictionary<(EnemyType, int), StatUpgradeSO> upgradeMilestones = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;                              //SUBSCRIBING TO OnSceneLoaded METHOD HERE. LOOK THIS UP
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Dynamically load all StatUpgradeSO assets
        allUpgrades = Resources.LoadAll<StatUpgradeSO>("Upgrades");

        // Example: print them
        foreach (var upgrade in allUpgrades)
        {
            gameMessages.ShowMessage("Loaded upgrade: " + upgrade.upgradeName);
        }

        // Auto-register them into the upgradeMilestones dictionary
        foreach (var upgrade in allUpgrades)
        {
            var key = (upgrade.appliesToEnemy, upgrade.unlockKillCount);

            if (!upgradeMilestones.ContainsKey(key))
            {
                upgradeMilestones[key] = upgrade;
                gameMessages.ShowMessage($"Registered upgrade: {upgrade.upgradeName} for {key}");
            }
            else
            {
                gameMessages.ShowMessage($"Duplicate upgrade milestone: {key} already registered.");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RegisterEnemyKill(EnemyType type)
    {
        if (enemyKills.ContainsKey(type))
        {
            enemyKills[type]++;
        }
        else
        {
            enemyKills[type] = 1;
        }

        gameMessages.ShowMessage($"Killed {type}. Total: {enemyKills[type]}");

        var key = (type, GetKillCount(type));
        if (upgradeMilestones.ContainsKey(key))
        {
            player.GetComponent<PlayerController>().ApplyUpgrade(upgradeMilestones[key]);
            gameMessages.ShowMessage("Applied upgrade: " + upgradeMilestones[key].upgradeName);
        }
    }

    public int GetKillCount(EnemyType type)
    {
        return enemyKills.ContainsKey(type) ? enemyKills[type] : 0;
    }

    private GameObject FindInactiveObject(string name)
    {
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name == name && !obj.hideFlags.HasFlag(HideFlags.HideInHierarchy))
            {
                // Make sure it's part of the scene, not a prefab in Assets
                if (obj.scene.IsValid() && obj.scene.isLoaded)
                {
                    return obj;
                }
            }
        }

        return null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameOverScreen = FindInactiveObject("Game Over Screen");
        player = GameObject.Find("Player");

        GameObject messageManagerObject = GameObject.Find("Message Manager");

        if (messageManagerObject != null)
        {
            gameMessages = messageManagerObject.GetComponent<FloatingMessageSpawner>();
        }
        else
        {
            Debug.LogError("Message Manager GameObject not found!");
        }

    }

}
