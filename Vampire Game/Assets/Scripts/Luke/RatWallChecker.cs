using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWallChecker : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wall");
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Health_Script>().dealDamage(damage);
        }

        if (!collision.isTrigger)
        {
            GetComponentInParent<RatMovement>().turnAround();
        }
            
    }
}
