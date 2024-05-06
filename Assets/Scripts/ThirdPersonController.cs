using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public int maxLives = 3; // Maximum number of lives
    private int currentLives; // Current number of lives
    public Text livesText; // Reference to the UI Text component for displaying lives
    private Vector3 initialPosition; // Initial position of the player

    public string nextSceneName; // Name of the scene to load next

    private void Start()
    {
        currentLives = maxLives; // Initialize current lives to max lives
        UpdateLivesText(); // Update UI text initially
        initialPosition = transform.position; // Store initial position of the player
    }

    private void Update()
    {
        if (currentLives <= 0)
        {
            Die();
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Calculate movement direction based on input axes
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ);

        // Move the player in the calculated direction
        transform.Translate(moveDirection);
    }

    public void LoseLife()
    {
        currentLives--; // Decrease the current number of lives
        UpdateLivesText(); // Update UI text
        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            // Reset player position to initial position
            transform.position = initialPosition;
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("GameOverScene"); // Load the game over scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Briefcase"))
        {
            PickUpBriefcase();
        }
    }

    private void PickUpBriefcase()
    {
        Destroy(GameObject.FindGameObjectWithTag("Briefcase")); // Destroy the briefcase
        SceneManager.LoadScene(nextSceneName); // Load the next scene defined in the inspector
    }

    private void UpdateLivesText()
    {
        livesText.text = "Lives: " + currentLives.ToString(); // Update UI text with current lives count
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LoseLife(); // Call LoseLife() method when colliding with an enemy
        }
    }
}
