using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private EquipmentSO itemDrop;

    private void OnDestroy()
    {
        if (itemDrop != null)
        {
            Debug.Log($"No item loot assigned on {gameObject.name}");
        }

        if (Inventory.Instance == null )
        {
            Debug.Log("Inventory instance is null");
        }

        if (FloatingMessageSpawner.Instance == null )
        {
            Debug.Log("Message Spawner instance is null");
        }

        Inventory.Instance.AddItem(itemDrop);

        FloatingMessageSpawner.Instance.ShowMessage($"Added {itemDrop.equipmentName} to player inventory");
    }
}
