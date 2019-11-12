using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health_Script>().respawn();
        }
        else if(collision.GetComponent<Health_Script>() != null && !collision.GetComponent<Health_Script>().getDeathFieldVariable())
        {
            collision.GetComponent<Health_Script>().dealDamage(int.MaxValue);
        }
    }
}
