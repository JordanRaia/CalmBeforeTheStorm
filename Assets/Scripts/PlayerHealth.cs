using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 36;
    public int numOfHearts = 3;

    public Image[] hearts;
    public Sprite[] heartSprites; // Array of 12 sprites for each heart state

    private int healthPerHeart = 12;

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


    // public int maxHealth = 100;
    // private int currentHealth;

    // void Start()
    // {
    //     // Initialize health
    //     currentHealth = maxHealth;
    // }

    // // Function to handle taking damage
    // public void TakeDamage(int damage)
    // {
    //     currentHealth -= damage;
    //     Debug.Log("Player took damage, current health: " + currentHealth);

    //     if (currentHealth <= 0)
    //     {
    //         Die();
    //     }
    // }

    // // Function to handle player death
    // void Die()
    // {
    //     Debug.Log("Player died");
    //     // Add death logic here (e.g., respawn, reload level, or disable player)
    //     gameObject.SetActive(false);
    // }

    // // Function to heal the player (if needed)
    // public void Heal(int healAmount)
    // {
    //     currentHealth += healAmount;
    //     if (currentHealth > maxHealth)
    //     {
    //         currentHealth = maxHealth;
    //     }
    //     Debug.Log("Player healed, current health: " + currentHealth);
    // }
}
