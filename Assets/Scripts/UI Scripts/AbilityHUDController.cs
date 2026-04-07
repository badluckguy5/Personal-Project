using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform abilityGrid;
    [SerializeField] private GameObject abilitySlotPrefab;

    [Header("Layout")]
    [SerializeField] private int maxColumns = 9;
    [SerializeField] private int maxRows = 2;

    private List<AbilityHUDSlot> slots = new List<AbilityHUDSlot>();
    private int maxSlots => maxColumns * maxRows;

    private void Start()
    {
        RefreshHUD();
    }

    private void OnEnable()
    {
        TrySubscribe();
    }

    private void TrySubscribe()
    {
        if (PlayerEquipment.Instance != null)
        {
            PlayerEquipment.Instance.OnEquipmentChanged += RefreshHUD;
            RefreshHUD(); // Initial refresh
            Debug.Log("Successfully subscribed to PlayerEquipment");
        }
        else
        {
            Debug.Log("PlayerEquipment not ready, trying again in 0.5 seconds");
            Invoke(nameof(TrySubscribe), 0.5f);
        }
    }

    private void OnDisable()
    {
        Debug.Log($"AbilityHUDController OnDisable - Unsubscribing");


        if (PlayerEquipment.Instance != null)
        {
            PlayerEquipment.Instance.OnEquipmentChanged -= RefreshHUD;
        }
    }

    private void RefreshHUD()
    {
        var abilitiesTest = GetAllEquipmentAbilities();
        Debug.Log($"Found {abilitiesTest.Count} abilities");

        if (abilityGrid == null)
        {
            Debug.LogError("AbilityGrid not assigned in AbilityHUDController!");
            return;
        }

        Debug.Log("RefreshHUD called. Equipment count: " + GetAllEquipmentAbilities().Count);

        List<EquipmentAbility> abilities = GetAllEquipmentAbilities();

        while (slots.Count < maxSlots)
        {
            GameObject slotObj = Instantiate(abilitySlotPrefab, abilityGrid);
            AbilityHUDSlot slot = slotObj.GetComponent<AbilityHUDSlot>();
            slots.Add(slot);
        }

        //Update Slots
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < abilities.Count && abilities[i] != null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].SetAbility(abilities[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }

        }
    }

    private List<EquipmentAbility> GetAllEquipmentAbilities()
    {
        List<EquipmentAbility> abilities = new List<EquipmentAbility>();

        if (PlayerEquipment.Instance != null)
        {
            var equipped = PlayerEquipment.Instance.GetAllEquippedItems();

            foreach (var item in equipped.Values)
            {
                if (item != null && item.itemData != null && item.itemData.ability != null)
                {
                    abilities.Add(item.itemData.ability);
                }
            }

        }


        return abilities;
    }

}
