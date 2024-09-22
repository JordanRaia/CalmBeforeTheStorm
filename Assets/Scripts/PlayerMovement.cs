using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D playerCollider;

    Vector2 movement;
    Vector2 lastSafePosition;

    public LayerMask wallLayer; // Assign your wall's layer to this in the Inspector

    void Start()
    {
        if (playerCollider == null)
        {
            playerCollider = GetComponent<Collider2D>();
        }
        lastSafePosition = rb.position;
    }

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // Normalize to prevent diagonal speed boost

        // Animation handling
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        // Movement with raycast checks
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Check for collisions with walls in the direction of movement
        if (!IsCollidingWithWall(newPosition))
        {
            rb.MovePosition(newPosition);
        }

        // Check if the player is stuck inside a TilemapCollider2D
        if (IsStuckInTilemap())
        {
            // Teleport player back to last safe position
            rb.position = lastSafePosition;
        }
        else
        {
            // Update last safe position
            lastSafePosition = rb.position;
        }
    }

    bool IsCollidingWithWall(Vector2 targetPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movement, 0.1f, wallLayer);

        if (hit.collider != null)
        {
            // If there's a wall in the direction of movement, prevent movement
            return true;
        }
        return false;
    }

    bool IsStuckInTilemap()
    {
        // Check for overlapping colliders at the player's position
        Collider2D[] colliders = Physics2D.OverlapBoxAll(playerCollider.bounds.center, playerCollider.bounds.size, 0f, wallLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider is TilemapCollider2D && collider != playerCollider)
            {
                // The player is overlapping with a TilemapCollider2D
                return true;
            }
        }
        return false;
    }
}
