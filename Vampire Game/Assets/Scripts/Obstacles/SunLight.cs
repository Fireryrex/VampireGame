using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLight : MonoBehaviour
{
    public int damage = 1;
    public float time = 1;
    private float time2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        time2 = Time.time;
        if (collision.gameObject.layer == 12)
        {
            
                time2 = Time.time - time2;
                collision.GetComponent<Health_Script>().dealDamage(damage);
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            Debug.Log("Time " + Time.time);
            if (Time.time - time2 > time)
            {
                time2 = Time.time - time2;
                collision.GetComponent<Health_Script>().dealDamage(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
