using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    // Global alert state (read-only publicly)
    protected bool globalAlerted;
    public bool GetGlobalAlerted => globalAlerted;

    // Player indoors state (public getter, controlled setter)
    protected bool playerIndoors = false;
    public bool GetPlayerIndoors => playerIndoors;

    // Player last location (public getter, controlled setter)
    protected Vector3 playerLastLocation;
    public Vector3 GetPlayerLastLocation => playerLastLocation;

    public event Action<Vector3> OnPlayerWentIndoors;
    public event Action OnPlayerWentOutdoors;

    protected virtual void Awake()
    {
        // Ensure singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected virtual void Start()
    {

    }


    public void SetPlayerIndoors(FortController setter, bool value)
    {
        bool wasIndoors = playerIndoors;
        playerIndoors = value;

        if (!wasIndoors && playerIndoors) // player just went indoors
        {
            Debug.Log("Player just went indoors");
            if (playerLastLocation != null)
                OnPlayerWentIndoors?.Invoke(playerLastLocation);
        }
        else if (wasIndoors && !playerIndoors) // player just went outdoors
        {
            Debug.Log("Player just went outdoors");
            OnPlayerWentOutdoors?.Invoke();
        }
    }

    public void SetPlayerLastLocation(FortController setter, Vector3 value)
    {
        if (setter != null)
        {
            Debug.Log("Last location set");

            playerLastLocation = value;
        }
        else
        {
            Debug.LogWarning("Unauthorized attempt to set PlayerLastLocation");
        }
    }
}
