using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public GameObject gameOverScreen;
    private FloatingMessageSpawner gameMessages;

    private StatUpgradeSO[] allUpgrades;
    private UpgradeSystem upgradeSystem;

    private EnemyKillTracker killTracker = new EnemyKillTracker();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upgradeSystem = new UpgradeSystem();
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("[GameManager] GameOverScreen is missing!");
        }
        
        Time.timeScale = 0f;
    }

    public void RegisterEnemyKill(EnemyType type)
    {
        int killCount = killTracker.IncrementKillCount(type);

        gameMessages?.ShowMessage($"Killed {type}. Total: {killCount}");

        TryApplyUpgrade(type, killCount);
    }

    private void TryApplyUpgrade(EnemyType type, int killCount)
    {
        if (!upgradeSystem.TryGetUpgrade(type, killCount, out var upgrade))
        {
            return;
        }

        var stats = player?.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyUpgrade(upgrade);
            gameMessages?.ShowMessage($"Applied upgrade: {upgrade.upgradeName}");
        }
        else
        {
            Debug.LogWarning($"[GameManager] Player or PlayerStats component is missing! " +
                $"Could not apply upgrade '{upgrade.upgradeName}' for {type} kills.");
        }
    }

    public int GetKillCount(EnemyType type)
    {
        return killTracker.GetKillCount(type);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public void BindSceneReferences (GameObject player, GameObject gameOverScreen, FloatingMessageSpawner messageSpawner)
    {
        this.player = player;
        this.gameOverScreen = gameOverScreen;
        this.gameMessages = messageSpawner;
    }

}
