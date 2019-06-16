using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    //public PolygonCollider2D attack1;
    //public PolygonCollider2D attack2;
    public PolygonCollider2D attack31;
    public PolygonCollider2D attack32;

    public PolygonCollider2D[] attacks;

    private PolygonCollider2D localCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Create a polygon collider
        localCollider = gameObject.AddComponent<PolygonCollider2D>();
        localCollider.isTrigger = true; // Set as a trigger so it doesn't collide with our environment
        localCollider.pathCount = 0; // Clear auto-generated polygons
    }

    public enum hitBoxes
    {
        frame2Box,
        frame3Box,
        clear // special case to remove all boxes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter2D(Collider2D col)
     {
         Debug.Log("Collider hit something!");
     }

    public void setHitBox(hitBoxes val)
    {
        if (val != hitBoxes.clear)
        {
            localCollider.SetPath(0, attacks[(int)val].GetPath(0));
            return;
        }
        localCollider.pathCount = 0;
    }
}
