using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.3f;
    public float delayBetweenLines = 2f;

    private string[] lines;
    private int index;

    void Start()
    {
        // Ensure the dialogue box is hidden at the start
        gameObject.SetActive(false);
    }

    // Method to initialize and start the dialogue with passed lines
    public void StartDialogue(string[] newLines)
    {
        lines = newLines;
        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);  // Ensure the dialogue object is visible
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(delayBetweenLines);

        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);  // Hide the dialogue when it ends
        }
    }
}
