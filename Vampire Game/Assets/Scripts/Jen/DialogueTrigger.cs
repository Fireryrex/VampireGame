using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] conversation;

    public string songToStopOnStart; //name of song to stop on dialogue start
    public string songToTriggerOnStart; //name of song to trigger on dialogue start
    public string songToTriggerAfter; //name of song to trigger on dialogue finish

    private DialogueManager dm;

    public void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        dm.updateMusic(songToStopOnStart, songToTriggerOnStart, songToTriggerAfter);
        dm.StartDialogue(conversation);
    }
}
