using UnityEngine;

public class InventoryTesterAcc : MonoBehaviour
{
    [SerializeField] private EquipmentSO testNecklace;
    [SerializeField] private EquipmentSO testRing;

    private void Update()
    {
        // Press N to add necklace
        if (Input.GetKeyDown(KeyCode.N) && testNecklace != null)
        {
            Inventory.Instance.AddItem(testNecklace);
            Debug.Log("Added test necklace");
        }

        // Press L to add Ring
        if (Input.GetKeyDown(KeyCode.L) && testRing != null)
        {
            Inventory.Instance.AddItem(testRing);
            Debug.Log("Added test ring");
        }

        // Press R to remove last item of each type (optional)
        if (Input.GetKeyDown(KeyCode.R))
        {
            var necklace = Inventory.Instance.GetItemsByType(EquipmentType.Necklace);
            if (necklace.Count > 0)
                Inventory.Instance.RemoveItem(necklace[necklace.Count - 1]);

            var ring = Inventory.Instance.GetItemsByType(EquipmentType.Ring);
            if (ring.Count > 0)
                Inventory.Instance.RemoveItem(ring[ring.Count - 1]);
        }
    }
}