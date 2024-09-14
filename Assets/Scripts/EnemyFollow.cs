using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private Transform target;
    public float speed;
    private Rigidbody2D rb;
    private bool canMove = true; // To control movement during knockback

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
