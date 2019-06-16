using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Manager : MonoBehaviour
{
    public float moveSpeed;
    public float horizontalMove = 0f;
    public float dashDirection = 0f;
    public int vertdashMulti = 5;
    public int horidashMulti = 10;
    public bool jump = false;
    public bool dash = false;
    public Character_Controller controller;
    public RopeSystem ropesystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;
        dashDirection = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            ropesystem.Detatch();
        }
        if (Input.GetButtonDown("Dash"))
        {
            print("dash");
            dash = true;
        }
        if (Input.GetButtonDown("Grapple"))
        {
            ropesystem.Shoot(horizontalMove/moveSpeed, dashDirection);
        }
    }

    private void FixedUpdate()
    {
        if (dash)
        {
            controller.Dash(horidashMulti * horizontalMove * Time.fixedDeltaTime, vertdashMulti * moveSpeed * Time.fixedDeltaTime, dashDirection);
            dash = false;
        }
        else if (!dash)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
        }
    }
}
