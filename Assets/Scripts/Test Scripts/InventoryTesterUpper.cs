using UnityEngine;

public class InventoryTesterUpper : MonoBehaviour
{
    [SerializeField] private EquipmentSO testHelmet;
    [SerializeField] private EquipmentSO testChest;
    [SerializeField] private EquipmentSO testGloves;
    [SerializeField] private EquipmentSO testGloves2;


    private void Update()
    {
        // Press H to add helmet
        if (Input.GetKeyDown(KeyCode.H) && testHelmet != null)
        {
            Inventory.Instance.AddItem(testHelmet);
            Debug.Log("Added test helmet");
        }

        // Press C to add chest
        if (Input.GetKeyDown(KeyCode.C) && testChest != null)
        {
            Inventory.Instance.AddItem(testChest);
            Debug.Log("Added test chest");
        }

        // Press G to add gloves
        if (Input.GetKeyDown(KeyCode.G) && testGloves != null)
        {
            Inventory.Instance.AddItem(testGloves);
            Debug.Log("Added test gloves");
        }

        // Press G to add gloves 2
        if (Input.GetKeyDown(KeyCode.F) && testGloves2!= null)
        {
            Inventory.Instance.AddItem(testGloves2);
            Debug.Log("Added test gloves2");
        }

        // Press R to remove last item of each type (optional)
        if (Input.GetKeyDown(KeyCode.R))
        {
            var helmets = Inventory.Instance.GetItemsByType(EquipmentType.Head);
            if (helmets.Count > 0)
                Inventory.Instance.RemoveItem(helmets[helmets.Count - 1]);

            var chests = Inventory.Instance.GetItemsByType(EquipmentType.Chest);
            if (chests.Count > 0)
                Inventory.Instance.RemoveItem(chests[chests.Count - 1]);

            var gloves = Inventory.Instance.GetItemsByType(EquipmentType.Hands);
            if (gloves.Count > 0)
                Inventory.Instance.RemoveItem(gloves[gloves.Count - 1]);
        }
    }
}