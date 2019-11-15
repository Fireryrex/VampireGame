using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyThis : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        if(GameManager.player == null){
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
