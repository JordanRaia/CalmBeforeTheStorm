using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))] // Ensures an AudioSource is attached
public class ButtonSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Sprite Settings")]
    public Sprite normalSprite;  // Sprite for normal state
    public Sprite clickedSprite; // Sprite for clicked state

    [Header("Audio Settings")]
    public AudioClip clickSound; // Sound to play on click
    public bool playOnPointerDown = true; // Choose when to play the sound

    private Image buttonImage;
    private AudioSource audioSource;

    void Start()
    {
        // Get the Image component of the button
        buttonImage = GetComponent<Image>();

        // Set the default sprite (normal sprite)
        if (normalSprite != null)
            buttonImage.sprite = normalSprite;

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.playOnAwake = false; // We will play the sound manually

        // Optionally, set other AudioSource properties
        // e.g., audioSource.volume = 0.5f;
    }

    // This function is called when the mouse button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        // Change the sprite to the clicked one
        if (clickedSprite != null)
            buttonImage.sprite = clickedSprite;

        // Play sound if set to play on pointer down
        if (playOnPointerDown && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    // This function is called when the mouse button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // Revert the sprite back to normal
        if (normalSprite != null)
            buttonImage.sprite = normalSprite;

        // Play sound if set to play on pointer up
        if (!playOnPointerDown && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
