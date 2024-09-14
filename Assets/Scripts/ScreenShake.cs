using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.2f;

    private CameraFollow cameraFollow; // Reference to the CameraFollow script

    void Start()
    {
        // Get reference to the CameraFollow script attached to the same GameObject
        cameraFollow = GetComponent<CameraFollow>();
    }

    // Public method to trigger the shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Generate a random shake offset
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            // Apply the shake offset to the camera through CameraFollow
            if (cameraFollow != null)
            {
                cameraFollow.shakeOffset = new Vector3(offsetX, offsetY, 0);
            }

            elapsed += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        // Reset the shake offset back to zero after shaking
        if (cameraFollow != null)
        {
            cameraFollow.shakeOffset = Vector3.zero;
        }
    }
}
