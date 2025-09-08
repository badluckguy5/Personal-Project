using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player; // Reference to the player's position
    public float moveSpeed = 10f; // Speed at which the enemy moves

    public float maxHealth = 2f;
    private float currentHealth;

    public Health healthBarPrefab;
    private Health healthBarInstance;

    private Transform worldSpaceCanvasTransform;

    void Awake()
    {
        if (worldSpaceCanvasTransform == null)
        {
            Canvas canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
            {
                worldSpaceCanvasTransform = canvas.transform;
            }
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.Find("Player");

        healthBarInstance = Instantiate(healthBarPrefab, worldSpaceCanvasTransform);
        healthBarInstance.transform.localScale = Vector3.one; // Reset scale so it's visible
        healthBarInstance.target = transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position; // Direction towards player

            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // Move the enemy towards the player
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(collision.gameObject);  //Destroy player on contact

        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float normalizedHealth = currentHealth / maxHealth;
        healthBarInstance.SetHealth(normalizedHealth);

        healthBarInstance.gameObject.SetActive(normalizedHealth < 1f);

        if (currentHealth <= 0)
        {
            Destroy(healthBarInstance.gameObject);
            Destroy(gameObject); // Or trigger death animation
        }
    }
}
