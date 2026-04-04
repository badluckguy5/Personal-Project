using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerController : Creature
{
    private PlayerStats stats;
    private PlayerShooter shooter;
    private PlayerEquipment equipment;

    private Vector2 movementInput;

    private Rigidbody rb;

    public TMP_Text healthText;

    protected override void Awake()
    {
        base.Awake();

        stats = GetComponent<PlayerStats>();
        shooter = GetComponent<PlayerShooter>();
        rb = GetComponent<Rigidbody>();

        maxHealth = 3;

        
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnJump -= OnJump;
        }
    }

    protected override void Start()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnJump += OnJump;
        }

        currentHealth = maxHealth;

        healthBarInstance = healthBarPrefab;

        healthText.text = currentHealth.ToString(); ;

    }

    // Update is called once per frame
    void Update()
    {
        movementInput = InputManager.Instance.Movement;
        MovePlayer();
        shooter.PlayerShoot();

    }

    //Moves player using Input System
    void MovePlayer()
    { 
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y) * stats.GetSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        healthText.text = currentHealth.ToString();
    }

    protected override void TriggerDeath()
    {
        GameManager.Instance.GameOver();
    }

    private void OnMovement(Vector2 input)
    {
        movementInput = input;
    }

    private void OnJump()
    {

    }

    public Vector3 GetMovementDirection()
    {
        return new Vector3(movementInput.x, 0, movementInput.y).normalized;
    }


}
