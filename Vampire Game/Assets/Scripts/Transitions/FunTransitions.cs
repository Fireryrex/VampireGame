using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunTransitions : MonoBehaviour
{
    public string TransitionTo;
    public GameObject FadeinQuad;
    public float cooldown = 1;
    float cutoff = 1;
    public float transitionTime;
    GameObject gamemanager;

    private void Update() 
    {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(cooldown <= 0 && other.gameObject.tag == "Player")
        {
            GameManager.instance.TransitionScene(TransitionTo,transitionTime);
            StartCoroutine("Transition");
        }
    }

}
