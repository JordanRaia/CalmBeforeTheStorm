using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System; // Add this directive

public class PopupManager : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Reference to the PopupMessageText
    public float displayDuration = 2f;  // Time before fade-out starts
    public float fadeDuration = 1f;     // Duration of the fade-out effect
    public bool autoClose = true;       // Whether the popup should close automatically

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // Ensure the popup is hidden at the start
        gameObject.SetActive(false);
    }

    // Method to display the popup with a specific message
    public void ShowPopup(string message)
    {
        // Stop any ongoing fade-out
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        messageText.text = message;
        gameObject.SetActive(true);

        // Reset alpha to 1 (fully visible)
        canvasGroup.alpha = 1f;

        // Re-enable interaction
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // If autoClose is true, start the fade-out coroutine after displayDuration
        if (autoClose)
        {
            fadeCoroutine = StartCoroutine(FadeOutPopup());
        }
    }

    // Coroutine to fade out the popup
    private IEnumerator FadeOutPopup()
    {
        // Wait for the display duration (unscaled time)
        yield return new WaitForSecondsRealtime(displayDuration);

        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime; // Use unscaledDeltaTime
            // Calculate the alpha based on the elapsed time
            float newAlpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            canvasGroup.alpha = newAlpha;
            yield return null;
        }

        // Ensure the alpha is set to 0
        canvasGroup.alpha = 0f;

        // Disable interaction
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Clear the coroutine reference
        fadeCoroutine = null;
    }

    // Method to hide the popup immediately
    public void HidePopup()
    {
        // Stop any ongoing fade-out
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        // Reset alpha to 1 in case it was mid-fade
        canvasGroup.alpha = 1f;

        gameObject.SetActive(false);
    }
}
