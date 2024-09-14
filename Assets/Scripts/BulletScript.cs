using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public float knockbackForce = 100f;  // The base knockback force
    public int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // This method is called when the bullet collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object hit is tagged as "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the EnemyHealth component attached to the enemy
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);  // Deal damage to the enemy
            }

            // Apply knockback to the enemy
            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                // Calculate the knockback direction
                Vector2 knockbackDirection = (enemyRb.transform.position - transform.position).normalized;
                enemyRb.velocity = Vector2.zero;  // Clear current velocity
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);  // Apply knockback force

                // Disable movement during knockback
                EnemyFollow enemyFollow = collision.gameObject.GetComponent<EnemyFollow>();
                if (enemyFollow != null)
                {
                    enemyFollow.DisableMovementForKnockback(0.2f);  // Disable movement for knockback duration
                }
            }

            Destroy(gameObject);  // Destroy the bullet after it hits the enemy
        }
    }
}
