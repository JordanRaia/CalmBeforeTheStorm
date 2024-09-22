using UnityEngine;
using UnityEngine.Rendering.Universal; // For Light2D
using System.Collections;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage dealt to the player upon collision.")]
    public int damage = 12;

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
        projectileLight = GetComponent<Light2D>();
        projectileCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isFadingOut)
            return;

        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            StartCoroutine(FadeOutAndDestroy());
        }
        else if (other.CompareTag("Projectile"))
        {
            // Hit by player projectile, destroy this projectile
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            StartCoroutine(FadeOutAndDestroy());
        }
    }

    // Call this method when hit by a melee attack
    public void HandleMeleeHit()
    {
        if (isFadingOut)
            return;

        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        if (isFadingOut)
            yield break;

        isFadingOut = true;

        if (projectileCollider != null)
        {
            projectileCollider.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        float fadeDuration = 0.5f;
        float time = 0;

        float startLightIntensity = projectileLight != null ? projectileLight.intensity : 0;
        float startSpriteAlpha = spriteRenderer != null ? spriteRenderer.color.a : 1f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            if (projectileLight != null)
            {
                projectileLight.intensity = Mathf.Lerp(startLightIntensity, 0, t);
            }

            if (spriteRenderer != null)
            {
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(startSpriteAlpha, 0, t);
                spriteRenderer.color = newColor;
            }

            yield return null;
        }

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

        Destroy(gameObject);
    }
}
