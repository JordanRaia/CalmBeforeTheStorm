using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void Start()
    {
        //StartCoroutine(ExampleDialogueCoroutine());
    }

    IEnumerator ExampleDialogueCoroutine()
    {
        yield return new WaitForSeconds(1);

        string[] dialogueLines = {
            "This is example dialogue",
            "If you want to see how I called it check out the game controller",
            ":D"
        };

        // Call the StartDialogue method with your custom lines
        dialogueManager.StartDialogue(dialogueLines);
    }
}

