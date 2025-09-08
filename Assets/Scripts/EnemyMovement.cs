using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player; // Reference to the player's position
    public float moveSpeed = 10f; // Speed at which the enemy moves


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(collision.gameObject);  //Destroy player on contact

        }
    }

}
