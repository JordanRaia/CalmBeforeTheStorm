using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;  // The enemy's maximum health
    private int currentHealth;   // The enemy's current health

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;  // Initialize health to the maximum value at the start
    }

    // This function reduces the health by a given damage amount
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;    // Reduce the current health by the damage amount

        if (currentHealth <= 0)     // Check if health reaches zero or below
        {
            Die();  // Call the Die function to handle enemy death
        }
    }

    // This function is called when the enemy dies
    void Die()
    {
        // Optionally, destroy the enemy game object
        Destroy(gameObject);
    }
}
