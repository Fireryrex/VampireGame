using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatWallChecker : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            GetComponentInParent<RatMovement>().turnAround();
        }
            
    }
}
