using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal_Damage : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] string AttackName = "Player";

    //cooldown variables
    [SerializeField] float Cooldown = .5f;
    private bool onCooldown = false;
    private float time = 0f;

    private void Update()
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
        if (collision.CompareTag(AttackName) && !onCooldown)   //only the player can trigger if the spawn is off cooldown
        {
            float num = collision.GetComponent<Health_Script>().knockback;
            collision.GetComponent<Health_Script>().dealDamage(damage);
            if(transform.position.x < collision.transform.position.x)
            {
                if(transform.position.y < collision.transform.position.y)
                {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(num, num, 0));
                }
                else
                {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(num, num, 0));
                }
            }
            else
            {
                if (transform.position.y < collision.transform.position.y)
                {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(-num, num, 0));
                }
                else
                {
                    collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(-num, num, 0));
                }
            }
            
        }
    }
}