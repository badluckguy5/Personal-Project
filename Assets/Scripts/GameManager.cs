using System.Collections.Generic;
using System.IO;
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
    private ItemSystem itemSystem;

    private int levelIndex;
    private EnemyKillTracker killTracker = new EnemyKillTracker();

    private List<string> appliedUpgradeIDs = new List<string>();

    private List<string> PlayerEquippedItems = new List<string>();

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up save file path
            savePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        upgradeSystem = new UpgradeSystem();
        itemSystem = new ItemSystem();
    }

    public void SaveGame(int completedLevelIndex)
    {
        SaveData data = new SaveData();

        data.lastCompletedLevel = completedLevelIndex;
        data.SaveKillCounts(killTracker.GetKillCounts());
        data.appliedUpgradeIDs = new List<string>(appliedUpgradeIDs);
        data.SaveInventory(Inventory.Instance.GetInventory());
        data.playerEquippedIDs = PlayerEquipment.Instance.GetAllEquippedItemIDS();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Game saved to: {savePath}");

    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        levelIndex = data.lastCompletedLevel;
        Dictionary<EnemyType, int> loadedKills = data.LoadKillCounts();
        killTracker.SetKillCounts(loadedKills);

        appliedUpgradeIDs = data.appliedUpgradeIDs ?? new List<string>();

        Inventory.Instance.SetInventory(data.LoadInventory(itemSystem.GetAllItems()));

        PlayerEquippedItems = data.playerEquippedIDs;

        Debug.Log("Game loaded successfully");

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

        string upgradeID = upgrade.GetUpgradeID();

        if (appliedUpgradeIDs.Contains(upgradeID))
        {
            return;
        }

        var stats = player?.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyUpgrade(upgrade);
            appliedUpgradeIDs.Add(upgradeID);
            gameMessages?.ShowMessage($"Applied upgrade: {upgrade.upgradeName}");
        }
        else
        {
            Debug.LogWarning($"[GameManager] Player or PlayerStats component is missing! " +
                $"Could not apply upgrade '{upgrade.upgradeName}' for {type} kills.");
        }
    }

    public void TryAllUpgradesOnLoad()
    {
        foreach (string ID in appliedUpgradeIDs)
        {
            ReapplyUpgrade(ID);
        }

        Debug.Log("Upgrades Applied Successfully");
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

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    private void ReapplyUpgrade(string upgradeID)
    {
        if (upgradeSystem.TryGetUpgradeByID(upgradeID, out StatUpgradeSO upgrade))
        {
            var stats = player?.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.ApplyUpgrade(upgrade);
                Debug.Log($"Re-applied upgrade on load: {upgrade.upgradeName}");
            }
        }
        else
        {
            Debug.LogWarning($"Failed to find upgrade with ID: {upgradeID}");
        }
    }

    public EquipmentSO GetItemByID(string itemID)
    {
        return itemSystem?.GetItemByID(itemID);
    }

    public Dictionary<string, EquipmentSO> GetAllItems()
    {
        return itemSystem?.GetAllItems();
    }

    public void ReequipItemsAfterLoad()
    {
        if (PlayerEquippedItems != null && PlayerEquippedItems.Count > 0)
        {
            foreach (string ID in PlayerEquippedItems)
            {
                if (Inventory.Instance.CheckForItemID(ID, out ItemInstance item))
                {
                    PlayerEquipment.Instance.Equip(item);
                    Debug.Log($"Re-equipped: {item.itemData.equipmentName}");
                }
                else
                {
                    Debug.LogWarning($"Item ID {ID} not found in inventory!");
                }
            }
        } 
    }
}
