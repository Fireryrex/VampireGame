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
            collision.GetComponent<Health_Script>().dealDamage(damage);
        }
    }
}