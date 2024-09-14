using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        // Start with the rest period when the game begins
        StartRestPeriod();
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
    }

    void StartWave()
    {
        isResting = false;
        currentWave++;
        waveStatusText.text = "Wave " + currentWave;

        // not yet implemented
        // Spawn enemies for the wave
        // for (int i = 0; i < enemiesPerWave; i++)
        // {
        //     int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        //     int randomEnemy = Random.Range(0, enemies.Length);
        //     Instantiate(enemies[randomEnemy], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
        // }

        // temp, later change to when all enemies are defeated
        // Schedule next rest period after a few seconds
        Invoke("StartRestPeriod", 5f);  // Optional: Change the 5f if you want to modify wave duration
    }

    void StartRestPeriod()
    {
        isResting = true;
        restTimer = timeBetweenWaves;
        waveStatusText.text = "Rest: " + Mathf.Ceil(restTimer);
    }
}
