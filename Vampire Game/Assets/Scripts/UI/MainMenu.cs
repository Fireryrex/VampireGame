﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    
    public void PlayGame()
    {
        //can provide name of a scene (like "Level1") or a build index (like 1)

        Debug.Log("PLAY");
        SceneManager.LoadScene("Sewer Scene 2");  //loads next level in the queue

        //CURRENTLY only has Main Menu in the queue --> uncomment the line above once there is more than one thing in there
        //to add scenes to the queue, go to:
        //File --> Build Settings --> drag and drop scenes into the queue


    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Entered the hollow");
    }

    private void OnParticleCollision(GameObject other) {
        Debug.Log("Entered the sphere");
        other.GetComponentInChildren<ParticleSystem>().Play();
    }


}
