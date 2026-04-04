using UnityEngine;

public class EquipmentWindowController : MonoBehaviour
{
    [SerializeField] private GameObject equipmentWindow;

    private void Start()
    {
        if (equipmentWindow == null)
        {
            Debug.LogError("Equipment Window not assigned", this);
            enabled = false;
        }

        equipmentWindow.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleEquipment();
        }
    }

    private void ToggleEquipment()
    {
        if (equipmentWindow != null)
        {
            equipmentWindow.SetActive(!equipmentWindow.activeSelf);
        }
    }

    public void OpenEquipment()
    {
        equipmentWindow.SetActive(true);
    }

    public void CloseEquipment()
    {
        equipmentWindow.SetActive(false);
    }

}
