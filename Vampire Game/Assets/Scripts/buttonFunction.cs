using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonFunction : MonoBehaviour
{
    public GameObject inventoryUI;
public void setActivity(){
        if(gameObject.activeSelf == true){
            Debug.Log("Entered the if");
            inventoryUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else{
            Debug.Log("Entered the else");
        }

    }
}
