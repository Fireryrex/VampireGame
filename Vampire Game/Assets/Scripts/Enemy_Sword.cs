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
            for (int i = 0; i < enemyScript.spearAttacks.Length; i++)
            {
                enemyScript.spearAttacks[i].SetActive(false);
            }
            //Debug.Log("adflhjlka");
            enemyScript.startSwordAttacking();
        }
    }
}
