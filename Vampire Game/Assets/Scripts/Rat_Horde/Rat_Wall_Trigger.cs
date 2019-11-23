using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Wall_Trigger : MonoBehaviour
{
    [SerializeField] Rat_Wall[] ratWall;
    [SerializeField] Rat_Horde_AI bossAI;
    [SerializeField] BossHealthScript bossHP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && bossHP.getHealthPercent() > 0)
        {
            Debug.Log("TrIgGeReD");
            bossAI.startFight();
            for(int i = 0; i < ratWall.Length; ++i)
            {
                ratWall[i].moveRatWall1();
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
