using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Input : MonoBehaviour
{
    public float moveSpeed;
    public float horizontalMove = 0f;
    public float dashDirection = 0f;
    public int vertdashMulti = 5;
    public int horidashMulti = 10;
    public bool jump = false;
    public bool dash = false;
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
            print("dash");
            dash = true;
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
