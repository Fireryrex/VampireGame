using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Graphics : MonoBehaviour
{
    public Pathfinding.AIPath ai_path;

    private void Start()
    {
        ai_path = GetComponentInParent<Pathfinding.AIPath>();
    }
    private void Update()
    {
        if(ai_path.desiredVelocity.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (ai_path.desiredVelocity.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
