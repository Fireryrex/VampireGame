using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSwordEnemy_Jump : MonoBehaviour
{
    public SpearSwordEnemy_AI enemyScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            //ends any sword attack going on (probably safe to remove)
            for(int i = 0; i < enemyScript.swordAttacks.Length; i++)
            {
                enemyScript.swordAttacks[i].SetActive(false);
            }
            //starts jump attack
            enemyScript.jump();
        }
    }
}
