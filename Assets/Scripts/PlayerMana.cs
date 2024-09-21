using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour
{
    // Mana and mana bars
    public float mana = 12f; // Current mana
    public float maxMana = 12f * 3f; // Maximum mana
    public int numOfManaBars = 3; // Number of mana bars
    public Image[] manaBars;
    public Sprite[] manaSprites; // Array of sprites for each mana bar state
    private float manaPerBar = 12f; // Mana units per bar

    public float manaRegenRate = 1f; // Mana regenerated per second

    void Start()
    {
        // Ensure mana doesn't exceed maximum
        mana = manaPerBar * numOfManaBars;
    }

    void Update()
    {
        // Regenerate mana over time
        mana += manaRegenRate * Time.deltaTime;
        if (mana > maxMana)
        {
            mana = maxMana;
        }

        maxMana = numOfManaBars * manaPerBar;

        // Update mana UI
        for (int i = 0; i < manaBars.Length; i++)
        {
            if (i < numOfManaBars)
            {
                manaBars[i].enabled = true; // Enable mana bar if it's within the number of bars
                float barMana = Mathf.Clamp(mana - (i * manaPerBar), 0f, manaPerBar); // Mana for this bar

                // Calculate which sprite to use (based on bar mana)
                int spriteIndex = Mathf.FloorToInt((barMana / manaPerBar) * (manaSprites.Length - 1));
                manaBars[i].sprite = manaSprites[spriteIndex];
            }
            else
            {
                manaBars[i].enabled = false; // Disable bars outside of the numOfManaBars range
            }
        }
    }

    // Function to use mana
    public bool UseMana(float amount)
    {
        if (mana >= amount)
        {
            mana -= amount;
            return true; // Mana was successfully used
        }
        else
        {
            Debug.Log("Not enough mana!");
            return false; // Not enough mana
        }
    }

    // Function to regenerate mana immediately
    public void RegenerateMana(float amount)
    {
        mana += amount;
        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    // Function to increase maximum mana
    public void IncreaseMaxMana(float amount)
    {
        maxMana += amount;
        numOfManaBars = (int)(maxMana / manaPerBar); // Update number of mana bars
        mana = maxMana; // Optionally refill mana when max increases
    }
}
