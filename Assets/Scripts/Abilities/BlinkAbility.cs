using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "TeleportAbility", menuName = "Abilities/Teleport")]
public class BlinkAbility : EquipmentAbility
{
    [Header("Teleport Settings")]
    [SerializeField] private float teleportDistance = 3f;
    [SerializeField] private float cooldown = 3f;

    private PlayerController playerController;

    public override float GetCooldown() => cooldown;

    public override void Activate(PlayerController player)
    {
        ResetCooldown();
        playerController = player;

        InputManager.Instance.OnJump += Teleport;
    }

    public override void Deactivate(PlayerController player) 
    { 
        InputManager.Instance.OnJump -= Teleport;
        playerController = null;
    }

    private void Teleport()
    {
        if (playerController == null) return;

        if (!IsReady()) 
        {
            CooldownMessage();
            return;
        }

        RecordAbilityUse();

        Vector3 moveDirection = playerController.GetMovementDirection();

        if (moveDirection == Vector3.zero)
        {
            moveDirection = playerController.transform.forward;
        }

        Vector3 destination = playerController.transform.position + moveDirection * teleportDistance;

        playerController.transform.position = destination;

        Debug.Log($"Teleported to {destination}");
    }
}