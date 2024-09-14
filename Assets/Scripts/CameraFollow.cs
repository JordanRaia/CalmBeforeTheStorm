using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public Vector3 shakeOffset; // Offset added from the shake effect

    void Update()
    {
        // Follow the player's position, plus any shake offset
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z) + shakeOffset;
    }
}
