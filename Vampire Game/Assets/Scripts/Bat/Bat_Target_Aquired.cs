using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Target_Aquired : MonoBehaviour
{
    private Pathfinding.AIDestinationSetter AI_Setter;
    // Start is called before the first frame update
    void Start()
    {
        AI_Setter = GetComponentInParent<Pathfinding.AIDestinationSetter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AI_Setter.target = collision.transform;
        }
    }

/*    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AI_Setter.target = null;
        }
    }*/
}
