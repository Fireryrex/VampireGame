using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Object : MonoBehaviour
{
    [SerializeField] int timeToDestroyObject;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroyObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
