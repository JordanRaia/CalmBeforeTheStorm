using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    public float moveSpeed = 2f; // Speed at which the ghost moves
    public int damage = 12; // Damage dealt to the player on contact
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // Ensure the ghost's collider is set to trigger
        GetComponent<Collider2D>().isTrigger = true;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Move the ghost towards the player using Rigidbody2D
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 newPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
        }
    }

    // Detect trigger collision with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Try to get the PlayerHealth component and deal damage
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
