using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed;
    private Rigidbody2D rb;
    private bool canMove = true; // To control movement during knockback

    public int damageToDeal = 4;
    public float damageInterval = 2.0f;
    private bool isTouchingPlayer = false;
    private bool canDealDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            // Move towards the player using Rigidbody2D
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;

            // Rotate the enemy to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);  // Subtract 90 to align the sprite
        }

        // Check if the enemy is touching the player and is allowed to deal damage
        if (isTouchingPlayer && canDealDamage)
        {
            // Access the PlayerHealth script on the player and deal damage
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToDeal);
                StartCoroutine(DamageCooldown());
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

    // Coroutine to handle damage cooldown
    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageInterval); // Wait for the defined interval
        canDealDamage = true;
    }

    // Call this function to stop movement temporarily during knockback
    public void DisableMovementForKnockback(float knockbackDuration)
    {
        StartCoroutine(StopMovementCoroutine(knockbackDuration));
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        // Disable movement and reset velocity
        canMove = false;
        rb.velocity = Vector2.zero;

        // Wait for the knockback duration to finish
        yield return new WaitForSeconds(duration);

        // Re-enable movement after knockback
        canMove = true;
    }
}
