using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform enemy; // Reference to the enemy's Transform
    public Vector3 offset = new Vector3(0, 1, 0); // Offset to keep the health bar above the enemy

    void LateUpdate()
    {
        // Keep the health bar above the enemy and prevent it from rotating with the enemy
        transform.position = enemy.position + offset;
        transform.rotation = Quaternion.identity; // Keep the health bar from rotating
    }
}
