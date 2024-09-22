using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed;
    private Rigidbody2D rb;
    private bool canMove = true; // To control movement during knockback
    private bool isDead = false; // To track whether the enemy is dead

    public int damageToDeal = 4;
    public float damageInterval = 2.0f;
    private bool isTouchingPlayer = false;
    private bool canDealDamage = true;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = speed; // Set the agent's speed to the public variable

        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        // Make sure Rigidbody2D is Kinematic so it doesn't interfere with NavMeshAgent
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isDead)
        {
            agent.SetDestination(target.position);
            agent.speed = speed; // Update the agent's speed dynamically

            // Get the current movement direction based on agent velocity
            Vector2 movementDirection = new Vector2(agent.velocity.x, agent.velocity.y);

            if (movementDirection != Vector2.zero)
            {
                // Calculate the angle the sprite should rotate to face the movement direction
                float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                rb.rotation = angle - 90f; // Adjust by 90 degrees to align the top of the sprite
            }
        }

        // Check if the enemy is touching the player and is allowed to deal damage
        if (isTouchingPlayer && canDealDamage && !isDead)
        {
            // Access the PlayerHealth script on the player and deal damage
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToDeal);
            }
        }
    }

    // Collision detection with the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    // Continuous collision detection while in contact with the player
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    // Collision detection when exiting contact with the player
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false; // Player is no longer in contact
        }
    }

    // Call this function to stop movement temporarily during knockback
    public void DisableMovementForKnockback(float knockbackDuration)
    {
        if (!isDead) // Only stop movement if the enemy is not dead
        {
            StartCoroutine(StopMovementCoroutine(knockbackDuration));
        }
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        // Disable movement and reset velocity
        canMove = false;
        if (agent != null && agent.isOnNavMesh) // Ensure agent is still valid and on the NavMesh
        {
            agent.isStopped = true;
        }

        // Wait for the knockback duration to finish
        yield return new WaitForSeconds(duration);

        // Re-enable movement after knockback if the enemy is not dead
        if (!isDead && agent != null && agent.isOnNavMesh)
        {
            canMove = true;
            agent.isStopped = false;
        }
    }

    // Call this method from the EnemyHealth script when the enemy dies
    public void OnEnemyDeath()
    {
        isDead = true;  // Mark the enemy as dead

        // Stop all coroutines when the enemy dies to avoid issues after the enemy is destroyed
        StopAllCoroutines();

        if (agent != null && agent.isOnNavMesh) // Ensure agent is still valid and on the NavMesh
        {
            agent.isStopped = true;  // Stop the NavMeshAgent from moving
        }
    }
}
