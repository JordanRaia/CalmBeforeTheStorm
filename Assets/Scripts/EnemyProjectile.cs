using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D
using System.Collections;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage dealt to the player upon collision.")]
    public int damage = 12; // Adjust this value based on your health system (12 removes one heart)

    [Header("Effects")]
    [Tooltip("Prefab for hit effect (optional).")]
    public GameObject hitEffectPrefab;

    private Light2D projectileLight;
    private Collider2D projectileCollider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFadingOut = false;

    void Start()
    {
        // Get the Light2D component attached to this GameObject
        projectileLight = GetComponent<Light2D>();
        // Get the Collider2D component
        projectileCollider = GetComponent<Collider2D>();
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile is already fading out to prevent multiple triggers
        if (isFadingOut)
            return;

        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Damage the player
                playerHealth.TakeDamage(damage);
            }

            // Instantiate hit effect at the collision point (optional)
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            // Disable the SpriteRenderer to make the sprite disappear
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            // Start the fade-out and destroy process
            StartCoroutine(FadeOutAndDestroy());
        }
        else
        {
            // Optionally, start fade-out upon collision with other objects
            // StartCoroutine(FadeOutAndDestroy());
        }
    }

    IEnumerator FadeOutAndDestroy()
    {
        if (isFadingOut)
            yield break;

        isFadingOut = true;

        // Disable the collider to prevent further collisions
        if (projectileCollider != null)
        {
            projectileCollider.enabled = false;
        }

        // Stop the projectile's movement
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // Optional: Make it kinematic to prevent physics interactions
        }

        float fadeDuration = 0.5f;
        float time = 0;

        float startLightIntensity = projectileLight != null ? projectileLight.intensity : 0;
        float startSpriteAlpha = spriteRenderer != null ? spriteRenderer.color.a : 1f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            // Fade out the light intensity
            if (projectileLight != null)
            {
                projectileLight.intensity = Mathf.Lerp(startLightIntensity, 0, t);
            }

            // Fade out the sprite's alpha
            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(startSpriteAlpha, 0, t);
                spriteRenderer.color = newColor;
            }

            yield return null;
        }

        // Ensure the light intensity and sprite alpha are set to zero at the end
        if (projectileLight != null)
        {
            projectileLight.intensity = 0;
        }
        if (spriteRenderer != null)
        {
            Color newColor = spriteRenderer.color;
            newColor.a = 0;
            spriteRenderer.color = newColor;
        }

        // After fading out, destroy the projectile
        Destroy(gameObject);
    }
}
