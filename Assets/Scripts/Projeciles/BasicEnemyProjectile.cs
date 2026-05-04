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

    private int playerLayer = 6;
    private int obstacleLayer = 11;

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
        int otherLayer = other.gameObject.layer;

        if (otherLayer == playerLayer)
        {
            other.GetComponent<Creature>()?.TakeDamage(damage);
            Destroy(gameObject);
        }

        else if (otherLayer == obstacleLayer)
        {
            Destroy(gameObject);
        }

    }

}