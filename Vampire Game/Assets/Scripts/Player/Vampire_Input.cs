using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Input : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalMove = 0f;
    private float dashDirection = 0f;
    private float facing = 0f;
    public int vertdashMulti = 5;
    public int horidashMulti = 10;
    private bool jump = false;
    private bool holdJump = false;
    private bool dash = false;
    public Vampire_Controller controller;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        dashDirection = Input.GetAxisRaw("Vertical");
        if (horizontalMove != 0)
        {
            facing = Input.GetAxisRaw("Horizontal");
        }

        //sees if player presses jump
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButton("Jump"))
        {
            holdJump = true;
        }

        //like jump but dash
        if (Input.GetButtonDown("Dash"))
        {
            //print("dash");
            dash = true;
        }

        //like dash but attacc
        if (Input.GetButtonDown("Attack"))
        {
            //print("attacc");
            controller.Attack();
        }

        //switches from one weapon to another
        if (Input.GetButtonDown("Switch Weapons"))
        {
            Debug.Log("switch");
            controller.switchWeapons();
        }
    }

    private void FixedUpdate()
    {

        //lets controller know player is dashing
        if (dash)
        {
            controller.Dash(horidashMulti * facing * moveSpeed * Time.fixedDeltaTime, vertdashMulti * moveSpeed * Time.fixedDeltaTime, dashDirection);
            dash = false;
        }
        //lets controller know player is not dashing
        else if (!dash)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump, holdJump, facing);
            jump = false;
            holdJump = false;
        }
    }
}
