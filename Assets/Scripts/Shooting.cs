using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire;
    private float timer;
    public float timeBetweenFiring;

    public SpriteRenderer weaponSpriteRenderer;
    public Sprite rangedWeaponSprite;
    public Sprite meleeWeaponSprite;

    private bool isRangedWeapon = true;

    public float meleeRange = 1f;
    public LayerMask enemyLayer;
    public int meleeDamage = 10;

    public float meleeCooldown = 0.5f;
    private bool canMelee = true;
    private float meleeTimer = 0f;

    public float meleeSwingAngle = 45f;
    public float meleeSwingSpeed = 10f;
    private bool isSwinging = false;

    private Quaternion initialBulletRotation;

    public int rangedDamage = 10;
    public int manaCostPerShot = 5; // Mana cost for each ranged attack

    private PlayerMana playerMana; // Reference to the PlayerMana script
    public GameObject playerGameObject; // Reference to the player GameObject
    public GameObject smoke; // Reference to the smoke prefab


    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        weaponSpriteRenderer.sprite = rangedWeaponSprite;

        initialBulletRotation = Quaternion.Euler(0, 0, -90);
        bulletTransform.localRotation = initialBulletRotation;

        // Get the PlayerMana component from the player GameObject
        if (playerGameObject != null)
        {
            playerMana = playerGameObject.GetComponent<PlayerMana>();
            if (playerMana == null)
            {
                Debug.LogError("PlayerMana script not found on the player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned in the Shooting script!");
        }
    }

    public void IncreaseMeleeDamage(int amount)
    {
        meleeDamage += amount;
    }

    public void IncreaseRangedDamage(int amount)
    {
        rangedDamage += amount;
    }

    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if (!isSwinging)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
        }

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (!canMelee)
        {
            meleeTimer += Time.deltaTime;
            if (meleeTimer > meleeCooldown)
            {
                canMelee = true;
                meleeTimer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchWeapon();
        }

        // Shooting Logic
        if (isRangedWeapon && canFire)
        {
            if (Input.GetMouseButton(0))
            {
                // Check if the player has enough mana
                if (playerMana != null && playerMana.UseMana(manaCostPerShot))
                {
                    canFire = false;
                    GameObject newBullet = Instantiate(bullet, bulletTransform.position, Quaternion.identity);

                    BulletScript bulletScript = newBullet.GetComponent<BulletScript>();
                    if (bulletScript != null)
                    {
                        bulletScript.damage = rangedDamage;
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    // Not enough mana to fire, create smoke once
                    if (smoke != null)
                    {
                        Instantiate(smoke, bulletTransform.position, Quaternion.identity);
                    }
                    else
                    {
                        Debug.LogError("Smoke prefab is not assigned in the Shooting script!");
                    }
                }
            }
        }
        else if (!isRangedWeapon && canMelee)
        {
            if (Input.GetMouseButtonDown(0))
            {
                canMelee = false;
                StartCoroutine(SwingSword(rotZ));
                PerformMeleeAttack();
            }
        }
    }

    private IEnumerator SwingSword(float initialRotation)
    {
        isSwinging = true;

        for (float t = 0; t < 1; t += Time.deltaTime * meleeSwingSpeed)
        {
            float angle = Mathf.Lerp(0, meleeSwingAngle, t);
            bulletTransform.localRotation = initialBulletRotation * Quaternion.Euler(0, 0, -angle);
            yield return null;
        }

        for (float t = 0; t < 1; t += Time.deltaTime * meleeSwingSpeed)
        {
            float angle = Mathf.Lerp(meleeSwingAngle, 0, t);
            bulletTransform.localRotation = initialBulletRotation * Quaternion.Euler(0, 0, -angle);
            yield return null;
        }

        bulletTransform.localRotation = initialBulletRotation;
        isSwinging = false;
    }

    private void SwitchWeapon()
    {
        isRangedWeapon = !isRangedWeapon;

        if (isRangedWeapon)
        {
            weaponSpriteRenderer.sprite = rangedWeaponSprite;
        }
        else
        {
            weaponSpriteRenderer.sprite = meleeWeaponSprite;
        }
    }

    private void PerformMeleeAttack()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(bulletTransform.position, meleeRange, enemyLayer);

        foreach (Collider2D hitObject in hitObjects)
        {
            // Check if the hit object is an enemy
            EnemyHealth enemyHealth = hitObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(meleeDamage);
            }

            // Check if the hit object is an enemy projectile
            EnemyProjectile enemyProjectile = hitObject.GetComponent<EnemyProjectile>();
            if (enemyProjectile != null)
            {
                // Debug log to check if the projectile is hit
                Debug.Log("Enemy projectile hit by melee attack!");

                // Call the method to handle projectile being hit by melee
                enemyProjectile.HandleMeleeHit();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bulletTransform.position, meleeRange);
    }
}
