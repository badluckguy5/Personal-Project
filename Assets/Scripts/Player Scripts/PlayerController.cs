using UnityEngine;
using TMPro;

public class PlayerController : Creature
{
    private PlayerStats stats;
    private PlayerShooter shooter;

    public TMP_Text healthText;

    protected override void Awake()
    {
        base.Awake();

        stats = GetComponent<PlayerStats>();
        shooter = GetComponent<PlayerShooter>();

        maxHealth = 3;
    }

    protected override void Start()
    {
        currentHealth = maxHealth;

        healthBarInstance = healthBarPrefab;

        healthText.text = "" + currentHealth;

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        shooter.PlayerShoot();

    }

    //Moves player mased on keyboard input
    void MovePlayer()
    { 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical) * stats.GetSpeed * Time.deltaTime;

        transform.Translate(move, Space.World);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        healthText.text = "" + currentHealth;
    }

    protected override void TriggerDeath()
    {
        Time.timeScale = 0f;
        GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();
    }

}
