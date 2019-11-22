using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathField : MonoBehaviour
{
    [SerializeField] private int damage = 1; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(collision.tag);
            collision.transform.position = collision.GetComponent<VampireController2>().lastPlayerPosition;
            collision.GetComponent<Health_Script>().dealDamage(damage);
            
        }
        else if(collision.GetComponent<Health_Script>() != null && !collision.GetComponent<Health_Script>().getDeathFieldVariable())
        {
            collision.GetComponent<Health_Script>().dealDamage(int.MaxValue);
        }
    }
}
