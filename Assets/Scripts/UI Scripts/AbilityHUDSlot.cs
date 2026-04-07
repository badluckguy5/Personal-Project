using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityHUDSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private ItemTooltip tooltip;

    private EquipmentAbility ability;

    public void Start()
    {
        if (tooltip == null)
        {
            tooltip = FindAnyObjectByType<ItemTooltip>();
            if (tooltip == null)
            {
                Debug.LogError("ItemTooltip not found in scene!");
            }
        }
    }

    public void SetAbility(EquipmentAbility newAbility)
    {
        ability = newAbility;

        if (ability != null)
        {
            if (ability.icon != null)
            {
                iconImage.sprite = ability.icon;
            }

            iconImage.enabled = true;
            UpdateCooldownDisplay();
        }

        else
        {
            iconImage.enabled = false;
            cooldownOverlay.fillAmount = 0;
            cooldownText.text = "";
        }
    }

    private void Update()
    {
        if (ability != null)
        {
            UpdateCooldownDisplay();
        }
    }

    private void UpdateCooldownDisplay()
    {
        if (ability == null) return;

        float remainingPercent = ability.GetCooldownPercent();

        if (remainingPercent > 0)
        {
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = remainingPercent;
            }

            if (cooldownText != null)
            {
                cooldownText.text = ($"{ability.GetCooldownRemaining():F1}");
            }
            cooldownText.enabled = true;
        }

        else
        {
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = 0;
            }
            if (cooldownText != null)
            {
                cooldownText.text = "";
                cooldownText.enabled = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ability != null && tooltip != null)
        {
            tooltip.ShowAbility(ability, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.Hide();
        }
    }

}
