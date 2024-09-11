using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform

    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
}
