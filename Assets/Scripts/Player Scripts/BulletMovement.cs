using Unity.VisualScripting;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    void Update()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy or the ground
        if (other.CompareTag("Ground"))
        {

            Destroy(gameObject);  // Destroy the bullet GameObject

        }

        if (other.CompareTag("Enemy"))
        {
            Creature enemy = other.GetComponent<Creature>();

            if (enemy != null)
            {
                enemy.TakeDamage(1f); // Deal 1 damage
            }
            Destroy(gameObject);    //Destroy bullet

        }

    }

}
