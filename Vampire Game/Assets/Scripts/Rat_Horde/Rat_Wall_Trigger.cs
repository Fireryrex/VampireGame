using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Wall_Trigger : MonoBehaviour
{
    public Rat_Wall ratWall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TrIgGeReD");
        ratWall.moveRatWall1();
    }
}
