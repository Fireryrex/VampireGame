using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Input : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalMove = 0f;
    private float dashDirection = 0f;
    public int vertdashMulti = 5;
    public int horidashMulti = 10;
    private bool jump = false;
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
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (Input.GetButtonDown("Dash"))
        {
            //print("dash");
            dash = true;
        }
        if (Input.GetButtonDown("Attack"))
        {
            //print("attacc");
            controller.Attack();
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
