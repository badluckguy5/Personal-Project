using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportAbility", menuName = "Abilities/Teleport")]
public class BlinkAbility : EquipmentAbility
{
    [Header("Teleport Settings")]
    [SerializeField] private float teleportDistance = 3f;

    private PlayerController playerController;

    public override void Activate(PlayerController player)
    {
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