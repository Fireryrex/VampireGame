﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire_Controller : MonoBehaviour
{
    private float jumpSpeed = 300f;
    private int airJumps = 1;
    public int numOfJumps = 1;
    private int airDashes = 1;
    public int numOfDashes = 1;
    public Rigidbody2D RigidBody2D;
    public BoxCollider2D groundCheck;
    private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    //bools to check if some actions are allowed atm.
    public bool isGrounded = false;
    public bool isAttacking = false;
    //public bool canJumporRoll = true;
    //weapon array
    public GameObject[] weapons;
    //combo counter
    public int attackNum = 0;
    public GameObject currentAttack;
    private float startTime;
    private float attackLength;

    private void Awake()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            startTime += Time.deltaTime;
            if(startTime > attackLength)
            {
                currentAttack.SetActive(false);
                isAttacking = false;
            }
        }
    }

    //Moving logic
    public void Move(float move, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, RigidBody2D.velocity.y);
        RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

        if (jump && isGrounded)
        {
            print("jump");
            RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, 0);
            RigidBody2D.AddForce(new Vector2(RigidBody2D.velocity.x, jumpSpeed));
        }
        else if (jump && airJumps > 0)
        {
            RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, 0);
            RigidBody2D.AddForce(new Vector2(RigidBody2D.velocity.x, jumpSpeed));
            airJumps -= 1;
        }
    }

    //Dashing logic
    public void Dash(float move, float dashSpeed, float direction)
    {
        if (isGrounded)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, dashSpeed * 10f * direction);
            RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
        }
        else if (airDashes > 0)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, dashSpeed * 10f * direction);
            RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
            if (direction > 0)
            {
                airDashes -= 1;
            }
        }
    }

    //Groundcheck logic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        resetAirMovements();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
    
    //resets jump and dash values
    public void resetAirMovements()
    {
        airJumps = numOfJumps;
        airDashes = numOfDashes;
    }

    //Attack logic
    public void Attack()
    {
        switch(attackNum)
        {
            case 0:
                Debug.Log("attaccing");
                currentAttack = weapons[0];
                currentAttack.SetActive(true);
                startTime = 0.0f;
                attackLength = 1.0f;
                isAttacking = true;
                break;
            default:
                break;
        }
    }
}
