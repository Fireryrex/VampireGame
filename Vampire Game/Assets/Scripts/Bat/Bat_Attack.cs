using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Attack : MonoBehaviour
{
    private Animator bat_attack;
    private Rigidbody2D bat_body;
    private Pathfinding.AIDestinationSetter AI_Setter;
    private Transform bat_target;

    // Start is called before the first frame update
    //santi was here
    void Start()
    {
        AI_Setter = GetComponentInParent<Pathfinding.AIDestinationSetter>();
        bat_attack = GetComponentInChildren<Animator>();
        bat_body = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            bat_target = collision.transform;
            //bat_body.bodyType = RigidbodyType2D.Static;
            bat_attack.SetTrigger("See_Player");
            AI_Setter.target = null;
        }
    }

    public void endAnimation()
    {
        bat_attack.ResetTrigger("See_Player");
        AI_Setter.target = bat_target;
        //bat_body.bodyType = RigidbodyType2D.Dynamic;

    }
}
