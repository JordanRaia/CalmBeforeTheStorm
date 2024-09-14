using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3 * 12;
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

    // Function to handle taking damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took damage, current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Function to handle player death
    void Die()
    {
        Debug.Log("Player died");
        // Add death logic here (e.g., respawn, reload level, or disable player)
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
        Debug.Log("Player healed, current health: " + health);
    }
}
