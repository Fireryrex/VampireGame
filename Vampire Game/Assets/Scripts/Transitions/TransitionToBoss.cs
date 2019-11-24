using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToBoss : MonoBehaviour
{
    private float cooldown = 1.0f;
    private void Update() {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(cooldown <= 0){
            other.transform.position = new Vector2(-8.68f, 0.5292202f);
            SceneManager.LoadScene("SewerBossFight");
        }
    }
}
