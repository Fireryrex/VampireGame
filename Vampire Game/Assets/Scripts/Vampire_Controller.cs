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
    private float movementSmoothing = .05f;
    private Vector3 m_Velocity = Vector3.zero;
    //bools to check if some actions are allowed atm.
    private bool isGrounded = false;
    private bool isAttacking = false;
    private bool attackInProgress = false;
    private bool additionalFrame = false;
    //public bool canJumporRoll = true;
    //weapon array
    public GameObject[] swordAttacks;
    public GameObject[] spearAttacks;
    //combo counter
    private int attackNum = 0;
    private GameObject currentAttack;
    private float startTime;
    private float attackLength;
    public float comboDelay = 1.0f;
    //cooldowns
    public float dashCooldown;
    private float dashCooldownStart = 100f;
    private bool dashCooldownActive = false;

    private int weapon = 1;
    public bool spearUnlocked = false;

    private void Awake()
    {
        RigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //attacking logic
        if (isAttacking)
        {
            startTime += Time.deltaTime;
            if(startTime > attackLength)
            {
                currentAttack.SetActive(false);
                attackInProgress = false;
                if (additionalFrame)
                {
                    Attack();
                }
            }
            if(startTime > comboDelay)
            {
                attackNum = 0;
                isAttacking = false;
            }
        }
        //dash cooldown timer
        if(dashCooldownActive)
        {
            dashCooldownStart += Time.deltaTime;
            if(dashCooldownStart > dashCooldown)
            {
                dashCooldownActive = false;
            }
        }
    }

    //Moving logic
    public void Move(float move, bool jump)
    {
        if (!attackInProgress)
        {
            //this is just flipping the player around
            if (move < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else if(move > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            //this is making the player move with the inputs
            Vector3 targetVelocity = new Vector2(move * 10f, RigidBody2D.velocity.y);
            RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);

            //jump bool is true meaning player is jumping and does jumping stuff
            if (jump && isGrounded) //normal jump
            {
                //print("jump");
                RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, 0);
                RigidBody2D.AddForce(new Vector2(RigidBody2D.velocity.x, jumpSpeed));
            }
            else if (jump && airJumps > 0) //air jump
            {
                RigidBody2D.velocity = new Vector2(RigidBody2D.velocity.x, 0);
                RigidBody2D.AddForce(new Vector2(RigidBody2D.velocity.x, jumpSpeed));
                airJumps -= 1;
            }
        }
    }

    //Dashing logic
    public void Dash(float move, float dashSpeed, float direction)
    {
        if (!attackInProgress && !dashCooldownActive)
        {
            if (isGrounded) //normal dash
            {
                Vector3 targetVelocity = new Vector2(move * 10f, dashSpeed * 10f * direction);
                RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
                dashCooldownStart = 0;
                dashCooldownActive = true;
            }
            else if (airDashes > 0) //air dash
            {
                Vector3 targetVelocity = new Vector2(move * 10f, dashSpeed * 10f * direction);
                RigidBody2D.velocity = Vector3.SmoothDamp(RigidBody2D.velocity, targetVelocity, ref m_Velocity, movementSmoothing);
                airDashes -= 1;
                dashCooldownStart = 0;
                dashCooldownActive = true;
            }
        }
    }

    //Groundcheck logic
    private void OnTriggerEnter2D(Collider2D collision) //when groundcheck collider collides with ground, isGrounded becomes True
    {
        if (collision.gameObject.layer == 11)
        {
            isGrounded = true;
            resetAirMovements();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) //when groundcheck collider leaves ground, isGrounded becomes False
    {
        if (collision.gameObject.layer == 11)
        {
            isGrounded = false;
        }
    }
    
    //resets jump and dash values
    public void resetAirMovements()
    {
        airJumps = numOfJumps;
        airDashes = numOfDashes;
    }

    //Attack logic (chooses which attack part of combo)
    public void Attack()
    {
        if (!attackInProgress && weapon == 1)
        {
            switch (attackNum)
            {
                case 0:
                    //Debug.Log("attaccing");
                    currentAttack = swordAttacks[0];
                    currentAttack.SetActive(true);
                    startTime = 0.0f;
                    attackLength = 0.3f;
                    isAttacking = true;
                    attackInProgress = true;
                    attackNum++;
                    break;
                case 1:
                    currentAttack = swordAttacks[1];
                    currentAttack.SetActive(true);
                    startTime = 0.0f;
                    attackLength = .2f;
                    isAttacking = true;
                    attackInProgress = true;
                    attackNum++;
                    break;
                case 2:
                    currentAttack = swordAttacks[2];
                    currentAttack.SetActive(true);
                    startTime = 0.0f;
                    attackLength = .05f;
                    isAttacking = true;
                    attackInProgress = true;
                    additionalFrame = true;
                    attackNum++;
                    break;
                case 3:
                    currentAttack = swordAttacks[3];
                    currentAttack.SetActive(true);
                    startTime = 0.0f;
                    attackLength = .7f;
                    isAttacking = true;
                    attackInProgress = true;
                    additionalFrame = false;
                    attackNum = 0;
                    break;
                default:
                    break;
            }
        }
        else if(!attackInProgress && weapon == 2)
        {
            if(isGrounded)
            {

            }
        }
    }

    public void switchWeapons()
    {
        if(weapon == 1 && spearUnlocked)
        {
            weapon = 2;
        }
        else
        {
            weapon = 1;
        }
    }
}
