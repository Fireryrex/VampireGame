using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartHolder : MonoBehaviour
{
    [SerializeField] GameObject heart;
    public void createHeart(){
        Instantiate(heart, gameObject.transform);
    }
}
