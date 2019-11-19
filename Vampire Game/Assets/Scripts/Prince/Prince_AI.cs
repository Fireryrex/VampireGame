using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince_AI : MonoBehaviour
{
    //Arena Gizmos
    [SerializeField] Transform center;
    [SerializeField] Transform[] teleportPositions;

    //Prince Stuff
    enum State { Dead, ChoosingAttack, Attacking }
    private State stateInt = 0;
    private Animator princeAnimator;
    private Rigidbody2D princeRigidBody;
    private Health_Script princeHealth;

    private Transform princeTransform;
    private GameObject player;

    [SerializeField] GameObject phaseOutParticle;

    //Teleport Stuff
    private int singleTeleportPosition;
    [SerializeField] float playerTeleportOffset;

//General Attack Stuff
    //Force Variables
    [SerializeField] float dashForce;
    [SerializeField] float slamForce;
    [SerializeField] float sideSlamForce;
    
    //Attack Logic Vars
    private int attack;
    private bool playerisRight;

    //Projectile Attack Vars
    [SerializeField] int numDiffProjectiles;
    private int chosenProjectile;
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        princeAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        princeRigidBody = GetComponent<Rigidbody2D>();
        princeHealth = GetComponentInChildren<Health_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stateInt == State.ChoosingAttack)
        {
            if(princeHealth.getHealthPercent() >= .8)
            {
                attack = Random.Range(0, 2);
                stateInt = State.Attacking;
            }
            else if(princeHealth.getHealthPercent() >= .5)
            {
                attack = Random.Range(0, 3);
                stateInt = State.Attacking;
            }
            else if(princeHealth.getHealthPercent() > 0)
            {
                attack = Random.Range(0, 5);
                stateInt = State.Attacking;
            }
            else
            {
                //Play death animation here
                stateInt = State.Dead;
            }
        }
    }

    public void phaseOut()
    {
        Instantiate(phaseOutParticle, transform);
    }



    public void teleport()
    {
        switch (attack)
        {
            //Dash Attack
            case 0:
                //go to the position that is opposite to the player
                if(player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    singleTeleportPosition = 0;
                }
                else
                {
                    playerisRight = false;
                    singleTeleportPosition = 1;
                }
                princeTransform.position = teleportPositions[singleTeleportPosition].position;
                //Start dash attack animation here
                princeAnimator.SetInteger("stateInt", 1);

                break;
            
            //Slam Attack
            case 1:
                singleTeleportPosition = 3;
                princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);

                break;

            //Projectile Attack
            case 2:
                if (player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    singleTeleportPosition = 0;
                }
                else
                {
                    playerisRight = false;
                    singleTeleportPosition = 1;
                }
                princeTransform.position = teleportPositions[singleTeleportPosition].position;

                break;

            //Sword Attack
            case 3:
                if (player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    singleTeleportPosition = 1;
                    princeTransform.position = new Vector3(player.transform.position.x - playerTeleportOffset, teleportPositions[singleTeleportPosition].position.y);
                }
                else
                {
                    playerisRight = false;
                    singleTeleportPosition = 1;
                    princeTransform.position = new Vector3(player.transform.position.x + playerTeleportOffset, teleportPositions[singleTeleportPosition].position.y);
                }

                break;
            
            //Diagonal Slam Attack
            case 4:
                if (player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    singleTeleportPosition = 3;
                    princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);
                }
                else
                {
                    playerisRight = false;
                    singleTeleportPosition = 3;
                    princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);
                }

                break;

            
            //Stab Attack
            case 5:
                if (player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    singleTeleportPosition = 0;
                    princeTransform.position = teleportPositions[singleTeleportPosition].position;
                }
                else
                {
                    playerisRight = false;
                    singleTeleportPosition = 1;
                    princeTransform.position = teleportPositions[singleTeleportPosition].position;
                }

                break;

            default:
                break;
        }
    }

//Dash attack functions
    //causes the prince to move
    public void dash()
    {
        if(playerisRight)
        {
            princeRigidBody.AddForce(Vector2.right * dashForce);
        }
        else
        {
            princeRigidBody.AddForce(Vector2.left * dashForce);
        }
    }

    //stops the prince from moving
    public void stopDash()
    {
        princeRigidBody.velocity = Vector2.zero;
    }

//Slam attack functions
    //causes the prince to slam down
    public void slam()
    {
        princeRigidBody.AddForce(Vector2.down * slamForce);
    }

//Sword attack functions
    

//Sideways slam attack functions

    public void sideSlam()
    {
        if (playerisRight)
        {
            princeRigidBody.AddForce(Vector2.left * sideSlamForce + Vector2.down * slamForce);
        }
        else
        {
            princeRigidBody.AddForce(Vector2.right * sideSlamForce + Vector2.down * slamForce);
        }
    }

//Stab attack functions


//Spawn attack functions
    public void startSpawn()
    {
        chosenProjectile = Random.Range(0, numDiffProjectiles - 1);
        isSpawning = true;
    }

    public void stopSpawn()
    {
        isSpawning = false;
    }

//Attack end function
    public void attackEnd()
    {
        stateInt = State.ChoosingAttack;
    }
}

