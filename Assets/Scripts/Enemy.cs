using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player; // Reference to the player's position
    public float moveSpeed = 10f; // Speed at which the enemy moves

    public float maxHealth = 2f;
    public float currentHealth;

    public Health healthBarPrefab;
    private Health healthBarInstance;

    private Transform worldSpaceCanvasTransform;

    public static int enemyCount = 1;

    void Awake()
    {
        if (worldSpaceCanvasTransform == null)          //Find UI Canvas
        {
            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (Canvas canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    worldSpaceCanvasTransform = canvas.transform;
                }
            }
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        player = GameObject.Find("Player");

        healthBarInstance = Instantiate(healthBarPrefab, worldSpaceCanvasTransform);
        healthBarInstance.name = "Health Bar " + enemyCount;

        healthBarInstance.transform.localScale = Vector3.one; // Ensure scale is reset
        healthBarInstance.transform.localPosition = Vector3.zero; // Ensure position is reset
        healthBarInstance.transform.localRotation = Quaternion.identity; // Ensure rotation is reset

        //healthBarInstance.transform.localScale = Vector3.one; // Reset scale so it's visible
        healthBarInstance.target = transform;

        gameObject.name = "Enemy " + enemyCount;
        enemyCount++;
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
            GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();


        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float normalizedHealth = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Or trigger death animation
            Destroy(healthBarInstance.gameObject);
            Debug.Log(healthBarInstance.name + " destroyed");

        }
        else if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(normalizedHealth);
        }
        
    }
}
