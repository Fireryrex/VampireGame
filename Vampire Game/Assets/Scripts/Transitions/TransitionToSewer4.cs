using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionToSewer4 : MonoBehaviour
{
    private float cooldown = 2.0f;
    private void Update() {
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
        if (cooldown <= 0){
            if (other.transform.position.x > 90.0f){
                other.transform.position = new Vector2(17.12f, 4.0f);
            }
            else{
                other.transform.position = new Vector2(342.87f, -1.64f);
                
            }
            SceneManager.LoadScene("Sewer Scene 4");
            
            
        }
    }
}
