using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sword : MonoBehaviour
{
    public Enemy_AI enemyScript;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("adflhjlka");
            enemyScript.startSwordAttacking();
        }
    }
}
