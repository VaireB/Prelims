using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform[] patrolPoints; 
    public float moveSpeed = 3f; 
    public float rotationSpeed = 5f; 
    public float detectionRange = 10f; 
    public float lineOfSightRange = 10f; 
    public LayerMask obstacleLayer; 
    public LayerMask playerLayer; 
    public float chaseSpeed = 10f; 

    private Transform player; 
    private Transform currentPatrolPoint; 
    private int currentPatrolIndex = 0; 
    private bool patrolling = true; 
    private bool playerDetected = false; 
    private int lives = 3; // Player's initial lives count
    private Vector3 initialPosition; // Initial position of the enemy

    void Start()
    {
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player object not found. Make sure the player has the 'Player' tag.");
        }

        initialPosition = transform.position; // Store initial position of the enemy
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange && CanSeePlayer())
        {
            playerDetected = true;
            patrolling = false;
        }
        else
        {
            playerDetected = false;
            patrolling = true;
        }

        if (patrolling)
        {
            Patrol();
        }
        else if (playerDetected)
        {
            Chase();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, lineOfSightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (Vector3.Distance(transform.position, player.position) <= lineOfSightRange)
                {
                    return true; 
                }
            }
        }
        return false; 
    }

    void Patrol()
    {
        Vector3 direction = currentPatrolPoint.position - transform.position;
        direction.y = 0f; 
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentPatrolPoint.position) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            currentPatrolPoint = patrolPoints[currentPatrolIndex];
        }
    }

    void Chase()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; 
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * chaseSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Decrement player's lives count
            lives--;

            // Check if the player has remaining lives
            if (lives <= 0)
            {
                // Load the game over scene
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                // Reset enemy's position to initial position
                transform.position = initialPosition;

                // Reset the current patrol index to start from the first patrol point
                currentPatrolIndex = 0;
                currentPatrolPoint = patrolPoints[currentPatrolIndex];
            }
        }
    }
}
