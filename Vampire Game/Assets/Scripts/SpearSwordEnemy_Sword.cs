using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSwordEnemy_Sword : MonoBehaviour
{
    public SpearSwordEnemy_AI enemyScript;

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
