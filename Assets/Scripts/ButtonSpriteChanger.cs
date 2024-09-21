using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSpriteChanger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Sprite normalSprite;  // Sprite for normal state
    public Sprite clickedSprite; // Sprite for clicked state

    private Image buttonImage;

    void Start()
    {
        // Get the Image component of the button
        buttonImage = GetComponent<Image>();

        // Set the default sprite (normal sprite)
        buttonImage.sprite = normalSprite;
    }

    // This function is called when the mouse button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        // Change the sprite to the clicked one
        buttonImage.sprite = clickedSprite;
    }

    // This function is called when the mouse button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // Revert the sprite back to normal
        buttonImage.sprite = normalSprite;
    }
}
