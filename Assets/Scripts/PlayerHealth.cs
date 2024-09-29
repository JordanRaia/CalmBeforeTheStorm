using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    // Hearts and health
    public int health = 3; // Start with 3 hearts worth of health
    public int numOfHearts = 3;
    public Image[] hearts;
    public Sprite[] heartSprites; // Array of 12 sprites for each heart state
    private int healthPerHeart = 12;

    // Damage red tint
    public Image damageOverlay;
    public float flashDuration = 0.5f;
    public GameObject deathUIPanel;

    // Invincibility flash
    public SpriteRenderer playerSprite;
    public float invincibilityDuration = 2f;
    public int flashCount = 10;
    private bool isInvincible = false;

    // Screen shake
    public ScreenShake screenShake;

    public int maxHearts = 10; // Maximum number of hearts allowed

    public WaveManager waveManager;  // Reference to the WaveManager script
    public TMP_Text waveReachedText;  // TextMeshPro for wave reached
    public TMP_Text enemiesKilledText;  // TextMeshPro for enemies killed

    // Audio components
    [Header("Audio Settings")]
    public AudioClip hurtSound; // Assign this in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Initialize health based on number of hearts
        health = health * healthPerHeart;

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from the player GameObject.");
        }

        // Ensure the WaveManager reference is set (you can assign it in the Inspector)
        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
        }
    }

    void Update()
    {
        if (health > numOfHearts * healthPerHeart)
        {
            health = numOfHearts * healthPerHeart;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numOfHearts)
            {
                hearts[i].enabled = true; // Enable heart if it's within the number of hearts
                int heartHealth = Mathf.Clamp(health - (i * healthPerHeart), 0, healthPerHeart); // Health for this heart

                // Calculate which sprite to use (based on heart health)
                int spriteIndex = Mathf.FloorToInt((heartHealth / (float)healthPerHeart) * (heartSprites.Length - 1));
                hearts[i].sprite = heartSprites[spriteIndex];
            }
            else
            {
                hearts[i].enabled = false; // Disable hearts outside of the numOfHearts range
            }
        }
    }

    // Function to handle taking damage
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        health -= damage;

        // Play hurt sound
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or hurtSound not assigned.");
        }

        // Flash screen on damage
        if (damageOverlay != null)
        {
            StartCoroutine(FlashDamageOverlay());
        }

        // Trigger screen shake on damage
        if (screenShake != null)
        {
            screenShake.TriggerShake(0.5f, 0.2f); // Adjust duration and magnitude as needed
        }

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFlash());
        }
    }

    // Coroutine to flash the red screen overlay
    private IEnumerator FlashDamageOverlay()
    {
        float elapsedTime = 0f;
        float halfFlashDuration = flashDuration / 2f; // Split the duration into two phases

        // First phase: Fade in from transparent to half-transparent red
        while (elapsedTime < halfFlashDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 0.12f, elapsedTime / halfFlashDuration);
            damageOverlay.color = new Color(1, 0, 0, alpha);
            yield return null; // Wait for the next frame
        }

        elapsedTime = 0f; // Reset elapsed time

        // Second phase: Fade out from half-transparent red to transparent
        while (elapsedTime < halfFlashDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.12f, 0, elapsedTime / halfFlashDuration);
            damageOverlay.color = new Color(1, 0, 0, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure the overlay is fully transparent at the end
        damageOverlay.color = new Color(1, 0, 0, 0);
    }

    // Coroutine for invincibility flash effect
    private IEnumerator InvincibilityFlash()
    {
        isInvincible = true; // Set invincibility to true
        float flashInterval = invincibilityDuration / (flashCount * 2); // Time between each flash

        for (int i = 0; i < flashCount; i++)
        {
            // Toggle sprite renderer off and on
            playerSprite.enabled = false; // Hide the player sprite
            yield return new WaitForSeconds(flashInterval);
            playerSprite.enabled = true; // Show the player sprite
            yield return new WaitForSeconds(flashInterval);
        }

        isInvincible = false; // End invincibility after flashing
    }

    // Function to handle player death
    void Die()
    {
        Debug.Log("Player died");

        // Set all heart sprites to the empty state
        foreach (Image heart in hearts)
        {
            heart.sprite = heartSprites[0];
        }

        // Keep the red damage overlay fully opaque
        if (damageOverlay != null)
        {
            damageOverlay.color = new Color(1, 0, 0, 0.12f); // Solid red tint
        }

        // Update the wave and enemies killed text
        if (waveManager != null)
        {
            waveReachedText.text = "Wave: " + waveManager.currentWave;
            enemiesKilledText.text = "Enemies Killed: " + waveManager.enemiesDefeated;
        }

        // Activate the death UI panel
        if (deathUIPanel != null)
        {
            deathUIPanel.SetActive(true); // Show the death UI
        }

        // Disable player controls or hide the player
        gameObject.SetActive(false);
    }


    // Function to heal the player (if needed)
    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > numOfHearts * healthPerHeart)
        {
            health = numOfHearts * healthPerHeart;
        }
    }

    public void IncreaseMaxHealth(int numHearts)
    {
        numOfHearts += numHearts;
        if (numOfHearts > maxHearts)
        {
            numOfHearts = maxHearts;
        }
        health = numOfHearts * healthPerHeart;
    }
}
