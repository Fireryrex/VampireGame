using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Groundcheck : MonoBehaviour
{
    public Vampire_Controller controller;

    private void Start()
    {
        controller = GetComponentInParent<Vampire_Controller>();
    }

    //Groundcheck logic
    private void OnTriggerStay2D(Collider2D collision) //when groundcheck collider collides with ground, isGrounded becomes True
    {
        if (collision.gameObject.layer == 11)
        {
            controller.groundFunction();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //when groundcheck collider leaves ground, isGrounded becomes False
    {
        if (collision.gameObject.layer == 11)
        {
            controller.airFunction();
        }
    }
}
