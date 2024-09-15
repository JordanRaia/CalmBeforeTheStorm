using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    private Collider2D playerCollider;
    private Vector3 lastSafePosition;

    void Start()
    {
        // If you don't have a Start method, add this one
        playerCollider = GetComponent<Collider2D>();
        lastSafePosition = transform.position;

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }


    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);

        // Add this code to save the last safe position and check if stuck
        if (!IsStuck())
        {
            lastSafePosition = transform.position;
        }
        else
        {
            FreePlayer();
        }
    }

    void FixedUpdate()
    {
        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    bool IsStuck()
    {
        Collider2D[] overlaps = new Collider2D[10];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(LayerMask.GetMask("tilemapCollider")); // Replace "Tilemap" with your tilemap's layer name
        int overlapCount = playerCollider.OverlapCollider(contactFilter, overlaps);
        return overlapCount > 0 && rb.velocity.magnitude < 0.1f;
    }

    void FreePlayer()
    {
        // Option 1: Teleport to last safe position
        transform.position = lastSafePosition;

        // Option 2: Apply an upward force (uncomment if you prefer this method)
        // rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
    }


}
