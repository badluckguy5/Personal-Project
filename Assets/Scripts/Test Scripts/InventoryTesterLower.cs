using UnityEngine;

public class InventoryTesterLower : MonoBehaviour
{
    [SerializeField] private EquipmentSO testPants;
    [SerializeField] private EquipmentSO testFeet;

    private void Update()
    {
        // Press P to add pants
        if (Input.GetKeyDown(KeyCode.P) && testPants != null)
        {
            Inventory.Instance.AddItem(testPants);
            Debug.Log("Added test pants");
        }

        // Press G to add shoes
        if (Input.GetKeyDown(KeyCode.F) && testFeet != null)
        {
            Inventory.Instance.AddItem(testFeet);
            Debug.Log("Added test shoes");
        }

        // Press R to remove last item of each type (optional)
        if (Input.GetKeyDown(KeyCode.R))
        {
            var pants = Inventory.Instance.GetItemsByType(EquipmentType.Legs);
            if (pants.Count > 0)
                Inventory.Instance.RemoveItem(pants[pants.Count - 1]);

            var shoes = Inventory.Instance.GetItemsByType(EquipmentType.Feet);
            if (shoes.Count > 0)
                Inventory.Instance.RemoveItem(shoes[shoes.Count - 1]);
        }
    }
}