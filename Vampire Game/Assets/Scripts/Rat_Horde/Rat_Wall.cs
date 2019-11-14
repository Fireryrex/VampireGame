using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Wall : MonoBehaviour
{
    public Transform wallMoveTo1;
    public Transform wallMoveTo2;
    [SerializeField] Transform wallMoveTo3;
    public float wallSpeed;
    [SerializeField] float wallSpeed2;
    private bool movingWall1 = false;
    private bool movingWall2 = false;
    private bool resetWall2 = false;
    private bool resetAllWalls = false;
    [SerializeField] bool isWall1 = false;

    private void Update()
    {
        if (movingWall1)
        {
            this.gameObject.transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(wallMoveTo1.transform.position.x, wallMoveTo1.transform.position.y, 0),
                        16 * Time.deltaTime
                    );
        }
        else if(movingWall2)
        {
            this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(wallMoveTo2.transform.position.x, wallMoveTo2.transform.position.y, 0),
                    wallSpeed2 * Time.deltaTime
                );
        }
        else if(resetWall2)
        {
            if (!isWall1)
            {
                this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(wallMoveTo1.transform.position.x, wallMoveTo1.transform.position.y, 0),
                    wallSpeed2 * Time.deltaTime
                );
            }
            else
            {
                this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(wallMoveTo3.transform.position.x, wallMoveTo3.transform.position.y, 0),
                    wallSpeed2 * Time.deltaTime
                );

            }
        }
    }

    //moves the rat wall to the starting position after the player enters the arena
    public void moveRatWall1()
    {
        movingWall1 = true;
        movingWall2 = false;
    }

    //moves the rat wall to the second position when the boss is at X amount of health
    public void moveRatWall2()
    {
        movingWall2 = true;
        movingWall1 = false;
    }

    public void resetWalls()
    {
        resetWall2 = true;
        movingWall1 = false;
        movingWall2 = false; 
    }
}
