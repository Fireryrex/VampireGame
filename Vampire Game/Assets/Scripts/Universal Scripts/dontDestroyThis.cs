using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyThis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.test == 0){
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
