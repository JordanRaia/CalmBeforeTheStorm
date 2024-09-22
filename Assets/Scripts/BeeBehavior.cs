using UnityEngine;
using System.Collections;

public class BeeBehavior : MonoBehaviour
{
    private Transform player;              // Reference to the player's transform
    public Vector3 offset = new Vector3(5, 5, 0); // Offset position (top right of the player)
    public float waitTime = 2f;           // Time to wait before charging
    public float chargeSpeed = 10f;       // Speed of the bee when charging
    public float returnSpeed = 5f;        // Speed when returning to idle position
    public float circleSpeed = 2f;        // Speed of circling movement

    private Vector3 idlePosition;         // Current idle position relative to the player

    private bool canDamage = false;       // Indicates if the bee can damage the player

    void Start()
    {
        // Find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player GameObject is tagged 'Player'.");
        }

        // Start the bee's behavior routine
        StartCoroutine(BeeRoutine());
    }

    IEnumerator BeeRoutine()
    {
        while (true)
        {
            // Update the idle position relative to the player's current position
            idlePosition = player.position + offset;

            // Move towards the idle position smoothly
            while (Vector3.Distance(transform.position, idlePosition) > 0.1f)
            {
                // Continuously update idlePosition in case the player moves
                idlePosition = player.position + offset;

                transform.position = Vector3.MoveTowards(transform.position, idlePosition, returnSpeed * Time.deltaTime);
                yield return null;
            }

            // Now, stay at the idle position, following the player's movement
            float timer = 0f;
            while (timer < waitTime)
            {
                idlePosition = player.position + offset;
                transform.position = idlePosition;
                timer += Time.deltaTime;
                yield return null;
            }

            // Record the player's position at the moment of charging
            Vector3 targetPosition = player.position;

            // Charge towards the player's current position
            yield return StartCoroutine(ChargeTowards(targetPosition));

            // Circle around and return to the idle position
            yield return StartCoroutine(CircleBack());

            // After circling back, move smoothly to the idle position
            yield return StartCoroutine(MoveToIdlePosition());
        }
    }

    IEnumerator ChargeTowards(Vector3 targetPosition)
    {
        canDamage = true; // Enable damage during charge

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chargeSpeed * Time.deltaTime);
            yield return null;
        }

        canDamage = false; // Disable damage after charge
    }

    IEnumerator CircleBack()
    {
        canDamage = true; // Enable damage during circle back

        // Calculate the direction from the bee's current position to the idle position
        Vector3 directionToIdle = idlePosition - transform.position;
        float distanceToIdle = directionToIdle.magnitude;

        // Determine the normal vector for the plane of the circle (perpendicular to the direction)
        Vector3 circleNormal = Vector3.forward; // For 2D, the normal is along the z-axis

        // Calculate the center point of the circle
        Vector3 centerPoint = transform.position + Vector3.Cross(directionToIdle.normalized, circleNormal) * (distanceToIdle / 2f);

        // Calculate the radius of the circle
        float radius = distanceToIdle / 2f;

        // Calculate the starting angle based on the bee's current position
        float startAngle = Mathf.Atan2(transform.position.y - centerPoint.y, transform.position.x - centerPoint.x);

        // For a half-circle
        float endAngle = startAngle + Mathf.PI;

        float angle = startAngle;
        float angleSpeed = circleSpeed / radius; // Adjust speed based on radius

        while (angle < endAngle)
        {
            // Move along the circle
            angle += angleSpeed * Time.deltaTime;

            float x = Mathf.Cos(angle) * radius + centerPoint.x;
            float y = Mathf.Sin(angle) * radius + centerPoint.y;

            transform.position = new Vector3(x, y, transform.position.z);

            yield return null;
        }

        canDamage = false; // Disable damage after circling back
    }

    IEnumerator MoveToIdlePosition()
    {
        // Update the idle position in case the player has moved
        idlePosition = player.position + offset;

        // Move towards the idle position smoothly
        while (Vector3.Distance(transform.position, idlePosition) > 0.1f)
        {
            idlePosition = player.position + offset;

            transform.position = Vector3.MoveTowards(transform.position, idlePosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Handle collisions with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Player"))
        {
            // Access the PlayerHealth script on the player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Adjust the damage amount as needed
            }
        }
    }
}
