using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    // Separate arrays for each level of enemies
    public GameObject[] level1Enemies;
    public GameObject[] level2Enemies;
    public GameObject[] level3Enemies;

    public GameObject[] coinPrefabs;  // List of different coin prefabs
    private Transform[] spawnPoints;
    public int enemiesPerWave = 5;
    public int coinBagsPerRest = 5;
    public float timeBetweenWaves = 15f;  // Rest period duration
    public Text waveStatusText; // UI Text to display the rest period countdown

    public int currentWave = 0;
    public int enemiesDefeated = 0;
    private bool isResting = true; // Start the game in resting state
    private float restTimer;

    // Reference to the SpawnManager
    public SpawnManager spawnManager;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> coinBags = new List<GameObject>();  // To track spawned coin bags

    public float spawnDelay = 2f; // Delay between each enemy spawn
    public DialogueManager dialogueManager;

    void Start()
    {
        // Start with the rest period when the game begins
        StartRestPeriod(25f);

        string[] dialogueLines = {
            "Welcome", "Press E to open Shop", "Press Space to switch between ranged and melee weapons", "Click to attack",
        };

        dialogueManager.StartDialogue(dialogueLines);
    }

    void Update()
    {
        if (isResting)
        {
            // Decrease the rest timer and update the UI with the remaining time
            restTimer -= Time.deltaTime;
            waveStatusText.text = "Rest: " + Mathf.Ceil(restTimer);

            if (restTimer <= 0)
            {
                StartWave();
            }
        }

        // Check if all enemies are defeated and start the rest period
        if (!isResting && activeEnemies.Count == 0)
        {
            StartRestPeriod();
        }
    }

    void StartWave()
    {
        // Remove coin bags at the start of the wave
        RemoveCoinBags();

        isResting = false;
        currentWave++;
        waveStatusText.text = "Wave " + currentWave;

        // Increment the number of enemies per wave by 1
        enemiesPerWave = 5 + currentWave - 1;

        // Determine which enemies to spawn based on the current wave
        List<GameObject> availableEnemies = new List<GameObject>();

        if (currentWave == 5 || currentWave == 10)
        {
            string[] dialogueLines = {
            "Stronger Enemies will now spawn",
            };

            dialogueManager.StartDialogue(dialogueLines);
        }

        if (currentWave >= 15 && currentWave % 5 == 0)
        {
            string[] dialogueLines = {
            "Enemies health and damage has been increased",
            };

            dialogueManager.StartDialogue(dialogueLines);
        }

        if (currentWave <= 5)
        {
            // First 5 waves: Only level 1 enemies
            availableEnemies.AddRange(level1Enemies);
        }
        else if (currentWave > 5 && currentWave <= 10)
        {
            // Waves 6-10: Level 1 and 2 enemies
            availableEnemies.AddRange(level1Enemies);
            availableEnemies.AddRange(level2Enemies);
        }
        else
        {
            // After wave 10: All levels
            availableEnemies.AddRange(level1Enemies);
            availableEnemies.AddRange(level2Enemies);
            availableEnemies.AddRange(level3Enemies);
        }

        // Start the coroutine to spawn enemies one by one
        StartCoroutine(SpawnEnemiesOneByOne(availableEnemies));
    }

    IEnumerator SpawnEnemiesOneByOne(List<GameObject> availableEnemies)
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 spawnPosition = spawnManager.GetValidSpawnPosition();
            int randomEnemy = Random.Range(0, availableEnemies.Count);
            GameObject spawnedEnemy = Instantiate(availableEnemies[randomEnemy], spawnPosition, Quaternion.identity);

            // Adjust enemy stats based on the wave
            AdjustEnemyStats(spawnedEnemy);

            // Add the spawned enemy to the list
            activeEnemies.Add(spawnedEnemy);

            // Register for the enemy's OnDeath event
            EnemyHealth enemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += OnEnemyDeath;
            }

            // Wait for the spawn delay before spawning the next enemy
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void StartRestPeriod(float time = 0f)
    {
        isResting = true;
        restTimer = timeBetweenWaves;
        if (time != 0f)
        {
            restTimer = time;
        }

        waveStatusText.text = "Rest: " + Mathf.Ceil(restTimer);

        // Spawn coin bags during the rest period
        SpawnCoinBags();
    }

    void OnEnemyDeath(GameObject enemy)
    {
        // Remove the enemy from the active list
        activeEnemies.Remove(enemy);
        enemiesDefeated += 1;
    }

    void SpawnCoinBags()
    {
        int coinMultiplier = GetCoinMultiplier(); // Calculate the coin multiplier

        for (int i = 0; i < coinBagsPerRest; i++)
        {
            Vector2 spawnPosition = spawnManager.GetValidSpawnPosition();
            int randomCoin = Random.Range(0, coinPrefabs.Length);
            GameObject coinBag = Instantiate(coinPrefabs[randomCoin], spawnPosition, Quaternion.identity);

            // Adjust the point value based on the coin multiplier
            CoinBag coinBagComponent = coinBag.GetComponent<CoinBag>();
            if (coinBagComponent != null)
            {
                coinBagComponent.pointValue *= coinMultiplier;
            }

            coinBags.Add(coinBag);
        }
    }

    void RemoveCoinBags()
    {
        // Destroy all coin bags at the end of the rest period
        foreach (GameObject coinBag in coinBags)
        {
            Destroy(coinBag);
        }
        coinBags.Clear();
    }

    private int GetCoinMultiplier()
    {
        return 1 << (currentWave / 5);
    }

    private void AdjustEnemyStats(GameObject enemy)
    {
        if (currentWave >= 15)
        {
            int numberOfIncrements = ((currentWave - 15) / 5) + 1;
            int healthIncrease = 50 * numberOfIncrements;
            int damageIncrease = 2 * numberOfIncrements;

            // Adjust the enemy's health
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.maxHealth += healthIncrease;
                enemyHealth.currentHealth = enemyHealth.maxHealth; // Ensure current health matches max health
            }

            // Adjust the enemy's damage based on the attached script
            // 1. EnemyFollow
            EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();
            if (enemyFollow != null)
            {
                enemyFollow.damageToDeal += damageIncrease;
            }

            // 2. GhostFollow
            GhostFollow ghostFollow = enemy.GetComponent<GhostFollow>();
            if (ghostFollow != null)
            {
                ghostFollow.damage += damageIncrease;
            }

            // 3. EnemyProjectile
            EnemyProjectile enemyProjectile = enemy.GetComponent<EnemyProjectile>();
            if (enemyProjectile != null)
            {
                enemyProjectile.damage += damageIncrease;
            }

            // 4. BeeBehavior
            BeeBehavior beeBehavior = enemy.GetComponent<BeeBehavior>();
            if (beeBehavior != null)
            {
                beeBehavior.damage += damageIncrease;
            }
        }
    }
}
