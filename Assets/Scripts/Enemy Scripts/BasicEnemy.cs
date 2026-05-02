using UnityEngine;

public class BasicEnemy : Creature
{
    private bool subscribedToEvents = false;

    protected GameObject player; // Reference to the player's position
    protected Vector3 targetPosition;
    protected bool chasingPlayer = false;

    [SerializeField]protected float baseSpeed = 5f;
    protected float moveSpeed = 5f; // Speed at which the enemy moves
    protected bool alerted = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (levelManager != null)
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

        if (levelManager != null)
        {
            levelManager.OnPlayerWentIndoors -= SetGoToLastKnownLocation;
            levelManager.OnPlayerWentIndoors -= CutSpeed;
            levelManager.OnPlayerWentOutdoors -= SetChasePlayer;
            levelManager.OnPlayerWentOutdoors -= RestoreSpeed;

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
            alerted = levelManager.GetGlobalAlerted;
        } 

        player = GameObject.Find("Player");

        if (levelManager.GetPlayerIndoors)
        {
            if (indoors)
            {
                chasingPlayer = true;
            }

            else
            {
                chasingPlayer = false;
                CutSpeed(default);
                targetPosition = levelManager.GetPlayerLastLocation;
            }
                
        }
        else
        {
            chasingPlayer = true;
        }
    }

    protected void Update()
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
        while (levelManager == null)
        {
            levelManager = FindAnyObjectByType<LevelManager>();
            yield return null; // wait for next frame
        }
        SubscribeToLevelManagerEvents();
    }

    private void SubscribeToLevelManagerEvents()
    {
        if (!subscribedToEvents)
        {
            levelManager.OnPlayerWentIndoors += SetGoToLastKnownLocation;
            levelManager.OnPlayerWentIndoors += CutSpeed;

            levelManager.OnPlayerWentOutdoors += SetChasePlayer;
            levelManager.OnPlayerWentOutdoors += RestoreSpeed;

            subscribedToEvents = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
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

    protected void SetGoToLastKnownLocation(Vector3 lastKnownPosition)
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

    protected void GoToLastKnownLocation()
    {
        Vector3 direction = targetPosition - transform.position; // Direction towards player

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // Move the enemy towards the last known location
    }

    protected void ChasePlayer()
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
