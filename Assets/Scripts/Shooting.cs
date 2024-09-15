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

    public SpriteRenderer weaponSpriteRenderer; // Reference to the sprite renderer for the weapon
    public Sprite rangedWeaponSprite; // Sprite for the ranged weapon
    public Sprite meleeWeaponSprite; // Sprite for the melee weapon

    private bool isRangedWeapon = true; // true for ranged, false for melee

    public float meleeRange = 1f; // Range for melee attack
    public LayerMask enemyLayer; // Layer for enemies to detect
    public int meleeDamage = 10; // Damage dealt by melee attack

    public float meleeCooldown = 1.0f; // Cooldown for melee attacks
    private bool canMelee = true; // Separate flag for melee attack availability
    private float meleeTimer = 0f; // Timer to track melee cooldown

    public float meleeSwingAngle = 45f; // The maximum angle of the sword swing
    public float meleeSwingSpeed = 10f; // Speed of the swing
    private bool isSwinging = false; // Track if a swing is in progress

    private Quaternion initialBulletRotation; // Store the initial rotation of the bullet (sword)

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        weaponSpriteRenderer.sprite = rangedWeaponSprite; // Set initial weapon sprite

        // Rotate the bullet sprite 90 degrees to the right
        initialBulletRotation = Quaternion.Euler(0, 0, -90);
        bulletTransform.localRotation = initialBulletRotation;
    }

    // Update is called once per frame
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

        // Handle melee attack cooldown
        if (!canMelee)
        {
            meleeTimer += Time.deltaTime;
            if (meleeTimer > meleeCooldown)
            {
                canMelee = true;
                meleeTimer = 0;
            }
        }

        // Switch between weapons when pressing the space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchWeapon();
        }

        // Shoot or perform melee attack depending on the current weapon
        if (Input.GetMouseButton(0))
        {
            if (isRangedWeapon && canFire)
            {
                canFire = false;
                Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            }
            else if (!isRangedWeapon && canMelee)
            {
                // Melee weapon attack logic here (e.g., close-range attack)
                canMelee = false;
                StartCoroutine(SwingSword(rotZ));
                PerformMeleeAttack();
            }
        }
    }

    // Coroutine for swinging the sword
    private IEnumerator SwingSword(float initialRotation)
    {
        isSwinging = true;

        // Swing forward relative to the initial bullet rotation
        for (float t = 0; t < 1; t += Time.deltaTime * meleeSwingSpeed)
        {
            float angle = Mathf.Lerp(0, meleeSwingAngle, t);
            bulletTransform.localRotation = initialBulletRotation * Quaternion.Euler(0, 0, -angle); // Apply rotation based on initial
            yield return null;
        }

        // Swing back to the initial position
        for (float t = 0; t < 1; t += Time.deltaTime * meleeSwingSpeed)
        {
            float angle = Mathf.Lerp(meleeSwingAngle, 0, t);
            bulletTransform.localRotation = initialBulletRotation * Quaternion.Euler(0, 0, -angle); // Return to initial rotation
            yield return null;
        }

        // Reset to the exact initial rotation after the swing
        bulletTransform.localRotation = initialBulletRotation;
        isSwinging = false;
    }

    // Switches between ranged and melee weapons
    private void SwitchWeapon()
    {
        isRangedWeapon = !isRangedWeapon;

        if (isRangedWeapon)
        {
            weaponSpriteRenderer.sprite = rangedWeaponSprite; // Change to ranged weapon sprite
        }
        else
        {
            weaponSpriteRenderer.sprite = meleeWeaponSprite; // Change to melee weapon sprite
        }
    }

    // Handles melee attack logic
    private void PerformMeleeAttack()
    {
        // Detect enemies within melee range around bulletTransform
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(bulletTransform.position, meleeRange, enemyLayer);

        // Damage each enemy hit by the melee attack
        foreach (Collider2D enemy in hitEnemies)
        {
            // Get the EnemyHealth component attached to the enemy
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(meleeDamage);  // Deal damage to the enemy
            }
        }

        Debug.Log("Melee attack performed! Hit " + hitEnemies.Length + " enemies.");
    }

    // Draw the melee range in the scene view (for debugging)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bulletTransform.position, meleeRange);
    }

}
