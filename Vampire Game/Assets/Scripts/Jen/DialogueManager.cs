using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private string songToStopOnStart; //name of song to stop on dialogue start
    private string songToTriggerOnStart; //name of song to trigger on dialogue start
    private string songToTriggerAfter; //name of song to trigger on dialogue finish

    public Animator animator;


    private Queue<Dialogue> dialogue_q;
    private bool dialogueActive;


    void Start()
    {
        dialogue_q = new Queue<Dialogue>();
    }

    private void Update()
    {
        if (dialogueActive && Input.GetButtonDown("Submit"))
        {
            DisplayNextDialogue();
        }
    }


    public void updateMusic(string stopStart, string triggerStart, string triggerAfter)
    {
        songToStopOnStart = stopStart;
        songToTriggerOnStart = triggerStart;
        songToTriggerAfter = triggerAfter;
    }


    public void StartDialogue(Dialogue[] conversation)
    {
        dialogueActive = true;
        //animator.SetBool("isOpen", true);

        if (songToStopOnStart != "NONE")
        {
            FindObjectOfType<AudioManager>().Stop(songToStopOnStart);
        }
        if (songToTriggerOnStart != "NONE")
        {
            FindObjectOfType<AudioManager>().Play(songToTriggerOnStart);
        }

        dialogue_q.Clear();

        foreach (Dialogue dialogue in conversation)
        {
            dialogue_q.Enqueue(dialogue);
        }

        DisplayNextDialogue();
    }

    public void DisplayNextDialogue()
    {
        if (dialogue_q.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue dialogue = dialogue_q.Dequeue();
        nameText.text = dialogue.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogue.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        //animator.SetBool("isOpen", false);

        if (songToTriggerOnStart != "NONE")
        {
            FindObjectOfType<AudioManager>().Stop(songToTriggerOnStart);
        }
        if (songToTriggerAfter != "NONE")
        {
            FindObjectOfType<AudioManager>().Play(songToTriggerAfter);
        }
    }


}
