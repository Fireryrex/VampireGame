using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Wall_Trigger : MonoBehaviour
{
    [SerializeField] Rat_Wall[] ratWall;
    [SerializeField] Rat_Horde_AI bossStart;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("TrIgGeReD");
            bossStart.startFight();
            for(int i = 0; i < ratWall.Length; ++i)
            {
                ratWall[i].moveRatWall1();
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
