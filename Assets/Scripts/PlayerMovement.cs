using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    public LayerMask wallLayer; // Assign your wall's layer to this in the Inspector

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
}
