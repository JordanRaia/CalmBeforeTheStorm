using UnityEngine;
using System.Collections;

public class PlantShooter : MonoBehaviour
{
    [Header("Timing Settings")]
    [Tooltip("Time between each attack.")]
    public float attackInterval = 2.0f;

    [Tooltip("Time the mouth stays open before firing.")]
    public float mouthOpenDelay = 0.5f;

    [Tooltip("Time the mouth stays open after firing.")]
    public float mouthOpenDuration = 0.5f;

    [Header("Sprites")]
    [Tooltip("Sprite when the mouth is closed.")]
    public Sprite mouthClosedSprite;

    [Tooltip("Sprite when the mouth is open.")]
    public Sprite mouthOpenSprite;

    [Header("Projectile Settings")]
    [Tooltip("Projectile prefab to instantiate.")]
    public GameObject projectilePrefab;

    [Tooltip("Speed of the projectile.")]
    public float projectileSpeed = 5f;

    [Header("References")]
    [Tooltip("Transform of the player to target.")]
    private Transform player;

    [Tooltip("Point from which the projectile is spawned.")]
    public Transform projectileSpawnPoint;

    private SpriteRenderer spriteRenderer;
    private float initialSpawnPointLocalX;
    private EnemyHealth enemyHealth;
    private Coroutine attackCoroutine;
    private bool isDead = false;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // Store the initial local X position of the projectile spawn point
        initialSpawnPointLocalX = projectileSpawnPoint.localPosition.x;

        // Get the EnemyHealth component
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath += HandleDeath;
        }

        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the attack routine
        StartCoroutine(AttackRoutine());
    }

    void Update()
    {
        if (!isDead)
        {
            // Flip the sprite based on player's position
            FlipSprite();
        }
    }

    void HandleDeath(GameObject enemy)
    {
        // Stop the attack routine
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        // Optionally, set isDead flag
        isDead = true;
    }

    void OnDestroy()
    {
        if (enemyHealth != null)
        {
            enemyHealth.OnDeath -= HandleDeath;
        }
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            // Wait for the specified interval before attacking
            yield return new WaitForSeconds(attackInterval);

            // Change sprite to mouth open
            spriteRenderer.sprite = mouthOpenSprite;

            // Optional delay before firing
            yield return new WaitForSeconds(mouthOpenDelay);

            // Fire the projectile
            FireProjectile();

            // Keep mouth open for a specified duration after firing
            yield return new WaitForSeconds(mouthOpenDuration);

            // Change sprite back to mouth closed
            spriteRenderer.sprite = mouthClosedSprite;
        }
    }

    void FireProjectile()
    {
        // Calculate the direction towards the player
        Vector3 direction = (player.position - projectileSpawnPoint.position).normalized;

        // Calculate the angle in degrees and adjust it so the top of the sprite points towards the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Instantiate the projectile with the calculated rotation
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.Euler(0, 0, angle));

        // Set the projectile's velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody2D component.");
        }
    }

    void FlipSprite()
    {
        if (player != null)
        {
            bool isPlayerToRight = player.position.x > transform.position.x;
            spriteRenderer.flipX = isPlayerToRight;

            // Adjust the projectile spawn point's local X position based on the initial value
            Vector3 spawnPointLocalPosition = projectileSpawnPoint.localPosition;
            spawnPointLocalPosition.x = initialSpawnPointLocalX * (isPlayerToRight ? -1 : 1);
            projectileSpawnPoint.localPosition = spawnPointLocalPosition;
        }
    }
}
