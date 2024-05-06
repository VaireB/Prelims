using UnityEngine;

public class Briefcase : MonoBehaviour
{
    public float rotationSpeed = 30f; // Adjust the rotation speed as needed
    public string playerTag = "Player"; // Tag assigned to the player GameObject

    private void Update()
    {
        // Rotate the briefcase around its y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Delete the briefcase GameObject
            Destroy(gameObject);
            // Trigger level completion
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        // Add any additional logic for level completion here
        Debug.Log("Level completed!");
        // For example, load the next level or show a victory screen
        // SceneManager.LoadScene("NextLevel");
    }
}
