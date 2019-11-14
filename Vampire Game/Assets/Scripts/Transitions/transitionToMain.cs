using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionToMain : MonoBehaviour
{
    private float cooldown = 1.0f;
    private void Update() {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (cooldown <= 2){
        SceneManager.LoadScene("Sewer Scene 2");
        }
    }
}
