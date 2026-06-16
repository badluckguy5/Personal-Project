using UnityEngine;

public class Creature : MonoBehaviour
{
    public float maxHealth = 2f;
    public float currentHealth;
    public bool indoors = false;

    public Health healthBarPrefab;
    protected Health healthBarInstance;

    private Transform worldSpaceCanvasTransform;

    public EnemyType enemyType;

    protected LevelManager levelManager;

    protected virtual void Awake()
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

    protected virtual void OnEnable()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.gameObject.SetActive(true);
        }
    }

    protected virtual void OnDisable()
    {
        if (healthBarInstance != null)
        {
            healthBarInstance.gameObject.SetActive(false);
        }
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        healthBarInstance = Instantiate(healthBarPrefab, worldSpaceCanvasTransform);

        healthBarInstance.transform.localScale = Vector3.one; // Ensure scale is reset
        healthBarInstance.transform.localPosition = Vector3.zero; // Ensure position is reset
        healthBarInstance.transform.localRotation = Quaternion.identity; // Ensure rotation is reset

        //healthBarInstance.transform.localScale = Vector3.one; // Reset scale so it's visible
        healthBarInstance.target = transform;

        WaitForLevelManager();

    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float normalizedHealth = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            TriggerDeath();

        }
        else if (healthBarInstance != null)
        {
            healthBarInstance.SetHealth(normalizedHealth);
        }
    }

    protected virtual void TriggerDeath()
    {
        GameManager.Instance.RegisterEnemyKill(enemyType);
        Destroy(gameObject); // Or trigger death animation
        Destroy(healthBarInstance.gameObject);

    }

    private System.Collections.IEnumerator WaitForLevelManager()
    {
        while (levelManager == null)
        {
            levelManager = FindAnyObjectByType<LevelManager>();
            yield return null; // wait for next frame
        }
    }


}
