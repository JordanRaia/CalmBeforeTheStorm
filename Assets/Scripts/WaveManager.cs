using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] spawnPoints;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 10f;  // Rest period duration
    public Text waveStatusText; // UI Text to display the rest period countdown

    private int currentWave = 0;
    private bool isResting = true; // Start the game in resting state
    private float restTimer;

    // Reference to the SpawnManager
    public SpawnManager spawnManager;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        // Start with the rest period when the game begins
        //StartRestPeriod();
        StartWave();
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
        isResting = false;
        currentWave++;
        waveStatusText.text = "Wave " + currentWave;

        // Spawn enemies for the wave
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector2 spawnPosition = spawnManager.GetValidSpawnPosition(); // Get position from SpawnManager
            int randomEnemy = Random.Range(0, enemies.Length);
            //randomEnemy = 11; //for testing new enemies
            GameObject spawnedEnemy = Instantiate(enemies[randomEnemy], spawnPosition, Quaternion.identity);

            // Add the spawned enemy to the list
            activeEnemies.Add(spawnedEnemy);

            // Register for the enemy's OnDeath event from the EnemyHealth component
            EnemyHealth enemyHealth = spawnedEnemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += OnEnemyDeath;
            }
        }
    }

    void StartRestPeriod()
    {
        isResting = true;
        restTimer = timeBetweenWaves;
        waveStatusText.text = "Rest: " + Mathf.Ceil(restTimer);
    }

    void OnEnemyDeath(GameObject enemy)
    {
        // Remove the enemy from the active list
        activeEnemies.Remove(enemy);
    }
}
