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
            if (collision.GetComponent<Health_Script>().health > 1)
            {
                collision.GetComponent<Health_Script>().dealDamage(damage);
                collision.transform.position = collision.GetComponent<VampireController2>().lastPlayerPosition;
                collision.GetComponent<Health_Script>().respawnAnimPlay();
                collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StartCoroutine(freezePlayerForABit(collision));
            }
            else
            {
                collision.transform.position = collision.GetComponent<VampireController2>().lastPlayerPosition;
                collision.GetComponent<Health_Script>().respawnAnimPlay();
                collision.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                StartCoroutine(freezePlayerForABit(collision));
                collision.GetComponent<Health_Script>().dealDamage(damage);
            }
        }
        else if(collision.GetComponent<Health_Script>() != null && !collision.GetComponent<Health_Script>().getDeathFieldVariable())
        {
            collision.GetComponent<Health_Script>().dealDamage(int.MaxValue);
        }
    }

    IEnumerator freezePlayerForABit(Collider2D player)
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<Health_Script>().respawnAnimEnd();
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
