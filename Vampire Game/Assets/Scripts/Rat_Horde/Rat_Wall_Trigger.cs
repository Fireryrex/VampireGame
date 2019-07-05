using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Wall_Trigger : MonoBehaviour
{
    public Rat_Wall[] ratWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TrIgGeReD");
        for(int i = 0; i < ratWall.Length; ++i)
        {
            ratWall[i].moveRatWall1();
        }
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
