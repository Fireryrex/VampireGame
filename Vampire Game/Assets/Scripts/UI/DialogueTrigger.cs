using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public bool hasBeenTriggered = false;
    public Dialogue[] conversation; 

    public string songToStopOnStart; //name of song to stop on dialogue start
    public string songToTriggerOnStart; //name of song to trigger on dialogue start
    public string songToTriggerAfter; //name of song to trigger on dialogue finish

    [SerializeField] private int id;
    private DialogueManager dm;

    public void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        hasBeenTriggered = GameManager.instance.checkCutSceneStatus(id);

        
    }

    public void TriggerDialogue()
    {
        dm.dialogueSystem.SetActive(true);
        //dm.updateMusic(songToStopOnStart, songToTriggerOnStart, songToTriggerAfter);
        dm.StartDialogue(conversation);
        
    }
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player"))
        {
            iDontCareWhatYouNameIt();
        }
    }

    public void iDontCareWhatYouNameIt()
    {
        if (hasBeenTriggered == false){
            Time.timeScale = 0f;
            GameManager.instance.changeCutSceneStatus(id);
            hasBeenTriggered = true;
            TriggerDialogue();
            
        }
    }

}
