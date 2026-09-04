using UnityEngine;

public class Detection : MonoBehaviour
{
    private float detectionRadius = 15f;
    private SphereCollider detectionCollider;

    void Awake()
    {
        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
    }

    // Upgrade method to expand detection range dynamically
    public void UpgradeDetection(float newRadius)
    {
        detectionRadius = newRadius;
        detectionCollider.radius = detectionRadius;
    }

    void OnTriggerEnter(Collider other)
    {
        BasicEnemy enemy = other.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            enemy.setAlerted();
        }

    }
}
