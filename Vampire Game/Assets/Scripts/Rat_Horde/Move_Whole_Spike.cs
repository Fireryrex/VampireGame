using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Whole_Spike : MonoBehaviour
{
    //moves all the spikes to their move up position
    public void moveUp(Transform location2)
    {
        transform.position = new Vector3(transform.position.x, location2.transform.position.y, 0);
    }
}
