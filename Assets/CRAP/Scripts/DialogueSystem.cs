using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.05f;

    private Queue<string> dialogueLines;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;
    private string currentLine;

    void Start()
    {
        dialogueLines = new Queue<string>();
        dialogueBox.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                dialogueText.text = currentLine; // Instantly display full text
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(string[] lines)
    {
        dialogueBox.SetActive(true);
        dialogueLines.Clear();
        foreach (string line in lines)
        {
            dialogueLines.Enqueue(line);
        }
        isDialogueActive = true;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentLine = dialogueLines.Dequeue();
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;
    }
}

public class NPC : MonoBehaviour
{
    public string npcName;
    public string[] npcDialogue;
    public GameObject interactionIcon;

    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        interactionIcon.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.StartDialogue(npcDialogue);
            interactionIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionIcon.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionIcon.SetActive(false);
            isPlayerInRange = false;
        }
    }
}