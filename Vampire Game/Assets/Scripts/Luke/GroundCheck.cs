﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("No Ground");
        if (collision.gameObject.layer == 11)
        {
            GetComponentInParent<RatMovement>().turnAround();
        }
    }
}
