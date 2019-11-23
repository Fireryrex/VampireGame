using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Graphics : MonoBehaviour
{
    private Pathfinding.AIPath ai_path;
    private Animator bat_Animation;

    private void Start()
    {
        bat_Animation = GetComponent<Animator>();
        AnimatorStateInfo state = bat_Animation.GetCurrentAnimatorStateInfo(0);
        bat_Animation.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        ai_path = GetComponentInParent<Pathfinding.AIPath>();
    }
    private void Update()
    {
        if (ai_path.desiredVelocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (ai_path.desiredVelocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
