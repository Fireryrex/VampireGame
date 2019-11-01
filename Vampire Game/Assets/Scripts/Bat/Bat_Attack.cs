using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Attack : MonoBehaviour
{
    //Movement and attacking stuff
    private Animator bat_attack;
    private Rigidbody2D bat_body;
    private Pathfinding.AIDestinationSetter AI_Setter;
    private Transform bat_target;

    //Attack Cooldown Stuff
    private bool onCooldown = false;
    private float time = 0f;
    [SerializeField] float Cooldown;                //MAKE SURE THIS IS GREATER THAN THE ANIMATION DURATION OR STUFF WILL BREAK

    //Attack Hitbox Stuff
    private PolygonCollider2D bat_bite;

    // Start is called before the first frame update
    void Start()
    {
        AI_Setter = GetComponentInParent<Pathfinding.AIDestinationSetter>();
        bat_attack = GetComponentInChildren<Animator>();
        bat_body = GetComponentInParent<Rigidbody2D>();
        bat_bite = GetComponentInChildren<PolygonCollider2D>();
        bat_bite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onCooldown)               //Cooldown counter
        {
            time += Time.deltaTime;
        }
        if (time >= Cooldown)
        {
            onCooldown = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bat_target = collision.transform;                       //Saves that targets transform for the bat movement system for later
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !onCooldown)                          //Sees if the target who entered the trigger is the player
        {
            //bat_body.bodyType = RigidbodyType2D.Static;
            bat_attack.SetTrigger("See_Player");                    //Starts animation
            AI_Setter.target = null;                                //Keeps bat still
            onCooldown = true;                                      //attack is on cooldown now.
            time = 0f;
        }
    }

    private void endAnimation()                                      //is called by animation event
    {
        bat_bite.enabled = false;
        bat_attack.ResetTrigger("See_Player");                      //Ends animation
        AI_Setter.target = bat_target;                              //takes saved transfom (of player) and sets it back so movement works again
        //bat_body.bodyType = RigidbodyType2D.Dynamic;

    }

    private void Create_Hitbox()
    {
        bat_bite.enabled = true;
    }
}
