using UnityEngine;

public class InventoryTesterWeapon : MonoBehaviour
{
    [SerializeField] private EquipmentSO testMainWep;
    [SerializeField] private EquipmentSO testOffWep;

    private void Update()
    {
        // Press M to add main hand wep
        if (Input.GetKeyDown(KeyCode.M) && testMainWep != null)
        {
            Inventory.Instance.AddItem(testMainWep);
            Debug.Log("Added test main hand wep");
        }

        // Press O to add offhand wep
        if (Input.GetKeyDown(KeyCode.O) && testOffWep != null)
        {
            Inventory.Instance.AddItem(testOffWep);
            Debug.Log("Added test offhand wep");
        }

        // Press R to remove last item of each type (optional)
        if (Input.GetKeyDown(KeyCode.R))
        {
            var mainHand = Inventory.Instance.GetItemsByType(EquipmentType.Weapon);
            if (mainHand.Count > 0)
                Inventory.Instance.RemoveItem(mainHand[mainHand.Count - 1]);

            var offHand = Inventory.Instance.GetItemsByType(EquipmentType.Offhand);
            if (offHand.Count > 0)
                Inventory.Instance.RemoveItem(offHand[offHand.Count - 1]);
        }
    }
}