using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleEvents : MonoBehaviour
{
     public void OnParticleCollision(GameObject other){
         if(other.CompareTag("Player")){
         Debug.Log("Player was touched");
         other.GetComponent<Health_Script>().currentBlood += 1;
         }
    }
}
