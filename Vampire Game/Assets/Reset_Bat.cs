using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Bat : MonoBehaviour
{
    private Bat_Target_Aquired bat_target_script;

    // Start is called before the first frame update
    void Start()
    {
        bat_target_script = GetComponentInChildren<Bat_Target_Aquired>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player Left");
            bat_target_script.Reset_Movement();
        }
    }
}
