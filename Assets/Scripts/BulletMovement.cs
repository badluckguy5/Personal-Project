using Unity.VisualScripting;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(1f); // Deal 1 damage
            }
            Destroy(gameObject);    //Destroy bullet

        }

    }

}
