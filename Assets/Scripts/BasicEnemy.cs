using UnityEngine;

public class BasicEnemy : Creature
{
    private GameObject player; // Reference to the player's position
    private float moveSpeed = 5f; // Speed at which the enemy moves


    protected override void Awake()
    {
        base.Awake();

        maxHealth = 2;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        player = GameObject.Find("Player");

    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position; // Direction towards player

            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // Move the enemy towards the player
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            CollisionDeath();

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;  // Stop any current movement
            }
        }
    }

}
