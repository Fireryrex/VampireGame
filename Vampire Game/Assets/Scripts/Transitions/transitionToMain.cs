using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionToMain : MonoBehaviour
{
    private float cooldown = 2.0f;
    private void Update() {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (cooldown <= 0){
            SceneManager.LoadScene("Sewer Scene 2");
        }
    }
}
