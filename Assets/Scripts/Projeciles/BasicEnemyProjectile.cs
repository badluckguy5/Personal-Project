using UnityEngine;

public class BasicEnemyProjectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float damage = 1f;

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float currDistance = 0f;
    [SerializeField] private float maxDistance = 20f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Initizalize(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy or the ground
        if (other.CompareTag("Ground"))
        {

            Destroy(gameObject);  // Destroy the bullet GameObject

        }

        if (other.CompareTag("Player"))
        {
            Creature player = other.GetComponent<Creature>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);

        }

    }

}
