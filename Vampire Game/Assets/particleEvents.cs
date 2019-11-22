using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEvents : MonoBehaviour
{
    private void Start() {
        float loopTime = 2.0f;
        while(loopTime >= 0){
            loopTime -= Time.deltaTime;
        }
        Destroy(GameObject.Find("BloodSplatter"));
    }
     public void OnParticleCollision(GameObject other){
         if(other.CompareTag("Player")){
            Debug.Log("Player was touched");
            other.GetComponent<Health_Script>().currentBlood += 1;
            
         }
    }
}
