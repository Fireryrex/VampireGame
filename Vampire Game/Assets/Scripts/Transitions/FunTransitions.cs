using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunTransitions : MonoBehaviour
{
    public string TransitionTo;
    public float cooldown = 0;
    float cutoff = 1;
    public float transitionTime;
    [SerializeField] Vector2 moveTo;
    GameObject gamemanager;

    private void Start()
    {
        cooldown = 0f;
    }

    private void Update() 
    {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(cooldown <= 0 && other.gameObject.tag == "Player")
        {
            GameManager.instance.TransitionScene(TransitionTo, transitionTime, moveTo);
        }
    }

}
