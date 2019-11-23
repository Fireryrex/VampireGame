using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInTransition : MonoBehaviour
{
    bool InOut;
    // Start is called before the first frame update
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void fadein()
    {
        anim.Play("FadeIn");
    }

    public void fadeout()
    {
        anim.Play("FadeIn");
    }

    public void setTimeScale(float timescale)
    {
        Time.timeScale = timescale;
    }

}
