using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;  // The enemy's maximum health
    private int currentHealth;   // The enemy's current health

    // Health bar sprite management
    public SpriteRenderer healthBarRenderer; // Reference to the SpriteRenderer for the health bar
    public Sprite[] healthBarSprites; // Array of sprites representing health stages (from empty to full)
    public float healthBarVisibleDuration = 2f; // How long the health bar stays visible after taking damage
    private float healthBarTimer = 0f; // Timer for hiding the health bar
    public float fadeDuration = 1f; // How long the fade out takes
    public int coinsOnDeath = 1;



    // Event to notify when the enemy dies
    public delegate void DeathEvent(GameObject enemy);
    public event DeathEvent OnDeath;

    private SpriteRenderer spriteRenderer; // Reference to the enemy's sprite renderer
    public Color hitColor = Color.red; // Color when hit
    public float flashDuration = 0.1f; // Duration of the flash

    private Color originalColor; // To store the original color of the health bar

    private bool isDying = false; // To track if the enemy is in the dying state
    private Collider2D enemyCollider; // Reference to the enemy's collider

    public int pointsOnDeath = 1;
    private PointsManager pointsManager;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;  // Initialize health to the maximum value at the start
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        UpdateHealthBar();
        healthBarRenderer.enabled = false;
        originalColor = healthBarRenderer.color;
        enemyCollider = GetComponent<Collider2D>(); // Get the Collider2D component

        // Automatically finds the PointsManager in the scene
        pointsManager = FindObjectOfType<PointsManager>();
    }

    void Update()
    {
        // Check if the health bar is visible and decrement the timer
        if (healthBarRenderer.enabled)
        {
            healthBarTimer -= Time.deltaTime;

            if (healthBarTimer <= 0 && healthBarRenderer.color.a > 0)
            {
                // Start fading out the health bar
                FadeOutHealthBar();
            }
        }
    }

    // Fade out the health bar over time
    private void FadeOutHealthBar()
    {
        // Calculate the alpha value based on the remaining fade time
        float fadeAlpha = Mathf.Clamp01(healthBarRenderer.color.a - (Time.deltaTime / fadeDuration));

        // Set the new color with the reduced alpha
        healthBarRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, fadeAlpha);

        // Once fully faded, disable the health bar
        if (fadeAlpha <= 0)
        {
            healthBarRenderer.enabled = false;
        }
    }

    // This function reduces the health by a given damage amount
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    // Reduce the current health by the damage amount

        StartCoroutine(FlashOnHit()); // Trigger a flash effect when taking damage
        UpdateHealthBar();
        ShowHealthBar();

        if (currentHealth <= 0 && !isDying)     // Check if health reaches zero or below
        {
            Die();  // Call the Die function to handle enemy death
        }
    }

    // Show the health bar and reset the timer
    private void ShowHealthBar()
    {
        healthBarRenderer.enabled = true; // Enable the health bar
        healthBarTimer = healthBarVisibleDuration; // Reset the timer to keep it visible for a short time
        healthBarRenderer.color = originalColor; // Reset the color to fully opaque
    }


    // Updates the health bar sprite based on the current health
    private void UpdateHealthBar()
    {
        // Calculate the health percentage
        float healthPercentage = (float)currentHealth / maxHealth;

        // Determine which sprite to use based on the health percentage
        int spriteIndex = Mathf.FloorToInt(healthPercentage * (healthBarSprites.Length - 1));

        // Clamp the index to ensure it's within the valid range
        spriteIndex = Mathf.Clamp(spriteIndex, 0, healthBarSprites.Length - 1);

        // Update the sprite renderer with the corresponding sprite
        healthBarRenderer.sprite = healthBarSprites[spriteIndex];
    }

    private IEnumerator FlashOnHit()
    {
        // Change the sprite color to indicate damage
        spriteRenderer.color = hitColor;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        // Revert the sprite color back to normal
        spriteRenderer.color = Color.white;
    }

    // This function is called when the enemy dies
    void Die()
    {
        isDying = true;

        pointsManager.AddPoints(pointsOnDeath);

        enemyCollider.enabled = false;

        // Notify the EnemyFollow script that the enemy is dead
        EnemyFollow enemyFollow = GetComponent<EnemyFollow>();
        if (enemyFollow != null)
        {
            enemyFollow.OnEnemyDeath();
        }

        // Notify any subscribers that this enemy is dead
        if (OnDeath != null)
        {
            OnDeath(gameObject);  // Trigger the OnDeath event and pass this enemy object
        }

        if (enemyFollow != null)
        {
            enemyFollow.OnEnemyDeath();
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float alpha = spriteRenderer.color.a;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            yield return null;
        }

        // Destroy the GameObject after fading out
        Destroy(gameObject);
    }

}
