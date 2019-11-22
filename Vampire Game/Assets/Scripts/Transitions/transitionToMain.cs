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
        if (cooldown <= 0){
            if(SceneManager.GetActiveScene().name == "Sewer Scene 1"){
                other.transform.position = new Vector2(0.88f, 1.0f);
            }
            else{
                other.transform.position = new Vector2(110.31f, 15.77f);
            }
            SceneManager.LoadScene("Sewer Scene 2");
        }
    }
}
