using UnityEngine;

public class CoinBag : MonoBehaviour
{
    public int pointValue = 1; // Points to add when collected

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Ensure the colliding object is the player
        {
            PointsManager pointsManager = FindObjectOfType<PointsManager>();
            if (pointsManager != null)
            {
                pointsManager.AddPoints(pointValue); // Add points to the PointsManager
            }

            Destroy(gameObject); // Destroy the coin bag
        }
    }
}
