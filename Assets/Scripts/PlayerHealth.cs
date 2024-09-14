using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public int numOfHearts = 3;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
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
