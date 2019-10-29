using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Attack : MonoBehaviour
{
    private RatMovement ratControl;

    private void Start()
    {
        ratControl = GetComponentInParent<RatMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ratControl.Attack();
    }
}
