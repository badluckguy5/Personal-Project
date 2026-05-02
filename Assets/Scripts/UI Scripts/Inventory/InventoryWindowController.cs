using UnityEngine;

public class InventoryWindowController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;

    private void Start()
    {
        /*
        if (inventoryWindow == null)
        {
            Debug.Log("Looking for Inventory Window...");

            inventoryWindow = GameObject.Find("Inventory Window");

            Debug.Log($"Find result: {(inventoryWindow != null ? "FOUND" : "NOT FOUND")}");
        }

        if (inventoryWindow == null)
        {
            Debug.LogError("Inventory Window not assigned", this);
            enabled = false;
        }
        */

        TrySet();

        inventoryWindow.SetActive(false);
    }

    private void TrySet()
    {
        // Find UI Canvas first
        GameObject uiCanvas = GameObject.Find("UI Canvas");
        if (uiCanvas != null)
        {
            Transform windowTransform = uiCanvas.transform.Find("Inventory Window");
            if (windowTransform != null)
            {
                inventoryWindow = windowTransform.gameObject;
                Debug.Log("Successfully found window");
                inventoryWindow.SetActive(false);
                return;
            }
        }

        Debug.Log("Window not found, trying again...");
        Invoke(nameof(TrySet), 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (inventoryWindow != null)
        {
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
        }
    }

    public void OpenInventory()
    {
        inventoryWindow.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryWindow.SetActive(false);
    }

    public void AssignInventoryWindow(GameObject window)
    {
        inventoryWindow = window;

        Debug.Log("Window Assigned");
    }

}
