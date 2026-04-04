using UnityEngine;

public class InventoryWindowController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryWindow;

    private void Start()
    {
        if (inventoryWindow == null)
        {
            Debug.LogError("Inventory Window not assigned", this);
            enabled = false;
        }

        inventoryWindow.SetActive(false);
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

}
