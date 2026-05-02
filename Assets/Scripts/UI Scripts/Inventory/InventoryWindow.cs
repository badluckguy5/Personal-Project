using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    public static InventoryWindow Instance { get; private set; }

    [SerializeField] private GameObject inventoryWindow;


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        TrySubscribe();

        DontDestroyOnLoad(gameObject);
    }

    private void TrySubscribe()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InventoryToggle += Toggle;

            Debug.Log("Successfully subscribed Inventory Window toggle");

            inventoryWindow.SetActive(false);
        }
        else
        {
            Debug.Log("Input Manager not ready, trying again in 0.5 seconds");
            Invoke(nameof(TrySubscribe), 0.5f);
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InventoryToggle -= Toggle;
        }
    }

    public void Toggle()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);
    }

    public void Show() => inventoryWindow.SetActive(true);
    public void Hide() => inventoryWindow.SetActive(false);

}
