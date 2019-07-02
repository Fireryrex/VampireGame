using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal_Damage : MonoBehaviour
{
    public int damage = 4;
    public string AttackName = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == AttackName)
        {
            float num = collision.GetComponent<Health_Script>().knockback;
            collision.GetComponent<Health_Script>().dealDamage(damage);
            if(transform.position.x < collision.transform.position.x)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(num, num, 0));
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector3(-num, num, 0));
            }
            
        }
    }
}