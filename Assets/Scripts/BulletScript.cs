using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;  // For Light2D
using UnityEngine.Tilemaps;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    public float force;
    public float knockbackForce = 100f;
    public int damage = 20;
    public Light2D bulletLight;  // Exposed in the Inspector
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        mainCam = Camera.main;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        spriteRenderer = GetComponent<SpriteRenderer>();

        // If bulletLight is not assigned, try to get it
        if (bulletLight == null)
        {
            bulletLight = GetComponentInChildren<Light2D>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Disable the SpriteRenderer to make the sprite disappear
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDirection = (enemyRb.transform.position - transform.position).normalized;
                Vector3 knockbackPosition = enemyRb.transform.position + (Vector3)(knockbackDirection * (knockbackForce * Time.fixedDeltaTime));
                enemyRb.MovePosition(knockbackPosition);

                EnemyFollow enemyFollow = collision.gameObject.GetComponent<EnemyFollow>();
                if (enemyFollow != null)
                {
                    enemyFollow.DisableMovementForKnockback(0.2f);
                }
            }

            // Disable collider and stop movement
            GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // Start the fade-out coroutine
            StartCoroutine(FadeOutAndDestroy());
        }
        // Check for collision with a TilemapCollider2D
        // else if (collision.GetComponent<TilemapCollider2D>() != null)
        // {
        //     // Disable the SpriteRenderer to make the sprite disappear
        //     if (spriteRenderer != null)
        //     {
        //         spriteRenderer.enabled = false;
        //     }

        //     // Disable collider and stop movement
        //     GetComponent<Collider2D>().enabled = false;
        //     GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //     // Start the fade-out coroutine
        //     StartCoroutine(FadeOutAndDestroy());
        // }
    }

    IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.5f;
        float time = 0;

        if (bulletLight != null)
        {
            float startIntensity = bulletLight.intensity;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                bulletLight.intensity = Mathf.Lerp(startIntensity, 0, time / fadeDuration);
                yield return null;
            }
        }
        else
        {
            // If bulletLight is null, wait for fadeDuration before destroying
            yield return new WaitForSeconds(fadeDuration);
        }

        Destroy(gameObject);
    }
}
