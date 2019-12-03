using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Anims : MonoBehaviour
{
    private Animator rat_Animation;

    // Start is called before the first frame update
    void Start()
    {
        rat_Animation = GetComponent<Animator>();
        AnimatorStateInfo state = rat_Animation.GetCurrentAnimatorStateInfo(0);
        rat_Animation.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
