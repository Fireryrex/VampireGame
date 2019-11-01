using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowFlyScript : MonoBehaviour
{
    private Animator crow_Animation;
    private Pathfinding.AIPath ai_path;

    // Start is called before the first frame update
    void Start()
    {
        crow_Animation = GetComponent<Animator>();
        AnimatorStateInfo state = crow_Animation.GetCurrentAnimatorStateInfo(0);
        crow_Animation.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        ai_path = GetComponentInParent<Pathfinding.AIPath>();
    }

    // Update is called once per frame
    void Update()
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
            private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            crow_Animation.SetTrigger("Touching Player");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            crow_Animation.ResetTrigger("Touching Player");
        }
    }
}
