using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireballSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from the fireball prefab.");
        }
    }

    void Start()
    {
        // Play the sound once when the fireball is instantiated
        if (audioSource != null && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
