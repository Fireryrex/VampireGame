using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Attack : MonoBehaviour
{
    private Animator bat_attack;

    // Start is called before the first frame update
    void Start()
    {
        bat_attack = GetComponentInChildren<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            bat_attack.SetTrigger("See_Player");
            Debug.Log("Penis");
        }
    }
}
