using UnityEngine;

public class BasicEnemy : Creature
{
    private bool subscribedToEvents = false;

    private GameObject player; // Reference to the player's position
    private Vector3 targetPosition;
    public bool chasingPlayer = false;

    private float baseSpeed = 5f;
    private float moveSpeed = 5f; // Speed at which the enemy moves
    private bool alerted = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (LevelManager.Instance != null)
        {
            SubscribeToLevelManagerEvents();
        }
        else
        {
            StartCoroutine(WaitForLevelManagerAndSubscribe());
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnPlayerWentIndoors -= SetGoToLastKnownLocation;
            LevelManager.Instance.OnPlayerWentIndoors += CutSpeed;
            LevelManager.Instance.OnPlayerWentOutdoors -= SetChasePlayer;
            LevelManager.Instance.OnPlayerWentOutdoors -= RestoreSpeed;

        }
    }

    protected override void Awake()
    {
        base.Awake();

        maxHealth = 2;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();

        if (!alerted)
        {
            alerted = LevelManager.Instance.GetGlobalAlerted;
        } 

        player = GameObject.Find("Player");

        if (LevelManager.Instance.GetPlayerIndoors)
        {
            if (indoors)
            {
                chasingPlayer = true;
            }

            else
            {
                chasingPlayer = false;
                CutSpeed(default);
                targetPosition = LevelManager.Instance.GetPlayerLastLocation;
            }
                
        }
        else
        {
            chasingPlayer = true;
        }
    }

    void Update()
    {
        if (player != null && alerted)
        {
            if (chasingPlayer)
            {
                ChasePlayer();
            }

            else
            {
                GoToLastKnownLocation();
            }
        }
    }

    private System.Collections.IEnumerator WaitForLevelManagerAndSubscribe()
    {
        while (LevelManager.Instance == null)
        {
            yield return null; // wait for next frame
        }
        SubscribeToLevelManagerEvents();
    }

    private void SubscribeToLevelManagerEvents()
    {
        if (!subscribedToEvents)
        {
            LevelManager.Instance.OnPlayerWentIndoors += SetGoToLastKnownLocation;
            LevelManager.Instance.OnPlayerWentIndoors += CutSpeed;

            LevelManager.Instance.OnPlayerWentOutdoors += SetChasePlayer;
            LevelManager.Instance.OnPlayerWentOutdoors += RestoreSpeed;

            subscribedToEvents = true;
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

    public void setAlerted()
    {
        alerted = true;
    }

    protected virtual void CollisionDeath()
    {
        Destroy(gameObject); // Or trigger death animation
        Destroy(healthBarInstance.gameObject);
    }

    private void SetGoToLastKnownLocation(Vector3 lastKnownPosition)
    {
        chasingPlayer = false;
        targetPosition = lastKnownPosition;
    }

    public void SetChasePlayer()
    {
        chasingPlayer = true;
        if (player != null)
        {
            targetPosition = player.transform.position;
        }
    }

    private void GoToLastKnownLocation()
    {
        Vector3 direction = targetPosition - transform.position; // Direction towards player

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // Move the enemy towards the last known location
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            targetPosition = player.transform.position;
        }

        Vector3 direction = targetPosition - transform.position; // Direction towards player

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // Move the enemy towards the player
    }

    public void CutSpeed(Vector3 _)
    {
        if (!indoors)
        {
            moveSpeed = moveSpeed / 3;
        }
    }

    public void RestoreSpeed()
    {
        if (!indoors)
        {
            moveSpeed = baseSpeed;
        }
    }
}
