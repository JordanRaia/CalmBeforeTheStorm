using UnityEngine;

public class CoinBag : MonoBehaviour
{
    [Tooltip("Points awarded when the coin is collected.")]
    public int pointValue = 1; // Points to add when collected

    [Tooltip("Sound to play when the coin is collected.")]
    public AudioClip collectSound; // Assign your collect sound here

    // Optional: Volume control for the sound effect
    [Range(0f, 1f)]
    public float soundVolume = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the colliding object is the player
        {
            // Play the collection sound at the CoinBag's position
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, soundVolume);
            }
            else
            {
                Debug.LogWarning("Collect sound not assigned in CoinBag script.");
            }

            // Add points to the PointsManager
            PointsManager pointsManager = FindObjectOfType<PointsManager>();
            if (pointsManager != null)
            {
                pointsManager.AddPoints(pointValue);
            }
            else
            {
                Debug.LogWarning("PointsManager not found in the scene.");
            }

            // Destroy the CoinBag GameObject
            Destroy(gameObject);
        }
    }
}
