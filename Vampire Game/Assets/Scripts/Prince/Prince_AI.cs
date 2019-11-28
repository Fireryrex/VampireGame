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

    //Attack Hitboxes
    enum Attack {dashAttackBox, slamAttackBox, swordAttackBox1, swordAttackBox2, stabAttackBox1, stabAttackBox2, diagonalAttackBox }
    [SerializeField] GameObject[] attackHitboxes;

    //Projectile Stuff
    [SerializeField] Transform[] projectileSpawnPoints;
    [SerializeField] float cooldown;
    private float currentCooldown;
    [SerializeField] GameObject[] spearProjectiles;
    [SerializeField] GameObject[] chakramProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        princeAnimator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        princeRigidBody = GetComponent<Rigidbody2D>();
        princeHealth = GetComponentInChildren<Health_Script>();
        princeTransform = GetComponent<Transform>();
        currentCooldown = cooldown;

    }

    // Update is called once per frame
    void Update()
    {
        if(stateInt == State.ChoosingAttack)
        {
            if(princeHealth.getHealthPercent() >= .8)
            {
                attack = Random.Range(0, 3);
                Debug.Log(attack);
                stateInt = State.Attacking;
            }
            else if(princeHealth.getHealthPercent() >= .5)
            {
                attack = Random.Range(0, 4);
                stateInt = State.Attacking;
            }
            else if(princeHealth.getHealthPercent() > 0)
            {
                attack = Random.Range(0, 6);
                stateInt = State.Attacking;
            }
            else
            {
                //Play death animation here
                stateInt = State.Dead;
                princeAnimator.SetInteger("stateInt", 0);
                GameManager.instance.returnCamera().m_Lens.OrthographicSize = 10;
                GameManager.instance.returnCamera().m_Follow = player.GetComponentInChildren<CameraReturnPoint>().returnThisTransform();
                Destroy(gameObject, 5f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isSpawning && currentCooldown <= 0)
        {
            spawnProjectile();
        }
        else if (isSpawning)
        {
            currentCooldown -= Time.fixedDeltaTime;
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
            //Slam Attack
            case 0:
                singleTeleportPosition = 2;
                princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);
                princeRigidBody.bodyType = RigidbodyType2D.Static;

                princeAnimator.SetInteger("stateInt", 2);
                break;

            //Dash Attack
            case 1:
                //go to the position that is opposite to the player
                int teleportChoice = Random.Range(0, 2);
                if(teleportChoice == 0)
                {
                    playerisRight = true;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    singleTeleportPosition = 0;
                }
                else
                {
                    playerisRight = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    singleTeleportPosition = 1;
                }
                Debug.Log(teleportPositions[singleTeleportPosition].position);
                princeTransform.position = teleportPositions[singleTeleportPosition].position;
                //Start dash attack animation here
                princeAnimator.SetInteger("stateInt", 1);

                break;

            //Projectile Attack
            case 2:
                teleportChoice = Random.Range(0, 2);
                if (teleportChoice == 0)
                { 
                    playerisRight = true;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    singleTeleportPosition = 0;
                }
                else
                {
                    playerisRight = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    singleTeleportPosition = 1;
                }
                princeTransform.position = teleportPositions[singleTeleportPosition].position;
                princeAnimator.SetInteger("stateInt", 3);

                break;

            //Sword Attack
            case 3:
                if (player.transform.position.x > center.transform.position.x)
                {
                    playerisRight = true;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    singleTeleportPosition = 1;
                    princeTransform.position = new Vector3(player.transform.position.x - playerTeleportOffset, teleportPositions[singleTeleportPosition].position.y);
                }
                else
                {
                    playerisRight = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    singleTeleportPosition = 1;
                    princeTransform.position = new Vector3(player.transform.position.x + playerTeleportOffset, teleportPositions[singleTeleportPosition].position.y);
                }
                princeAnimator.SetInteger("stateInt", 4);

                break;
            
            //Diagonal Slam Attack
            case 4:
                princeRigidBody.bodyType = RigidbodyType2D.Static;
                if (player.transform.position.x < center.position.x)
                {
                    playerisRight = true;
                    transform.rotation = Quaternion.Euler(0, -0, 0);
                    singleTeleportPosition = 2;
                    princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);
                }
                else
                {
                    playerisRight = false;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    singleTeleportPosition = 2;
                    princeTransform.position = new Vector3(player.transform.position.x, teleportPositions[singleTeleportPosition].position.y);
                }
                princeAnimator.SetInteger("stateInt", 5);

                break;

            
            //Stab Attack
            case 5:
                teleportChoice = Random.Range(0, 2);
                if (teleportChoice == 0)
                {
                    playerisRight = true;
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    singleTeleportPosition = 0;
                    princeTransform.position = teleportPositions[singleTeleportPosition].position;
                }
                else
                {
                    playerisRight = false;
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    singleTeleportPosition = 1;
                    princeTransform.position = teleportPositions[singleTeleportPosition].position;
                }
                princeAnimator.SetInteger("stateInt", 6);

                break;

            default:
                break;
        }
    }

//Beginning of fight function
    public void startFight(int firstAttack)
    {
        princeAnimator.SetInteger("stateInt", firstAttack);
    }

//Dash attack functions
    //causes the prince to move
    public void dash()
    {
        if(playerisRight)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
            princeRigidBody.AddForce(Vector2.right * 1000 * dashForce);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            princeRigidBody.AddForce(Vector2.left * 1000 * dashForce);
        }
        activateHitbox(0);
    }

    //stops the prince from moving
    public void stopDash()
    {
        princeRigidBody.velocity = Vector2.zero;
        deactivateHitbox(0);
        deactivateHitbox(1);
        deactivateHitbox(6);
    }

//Slam attack functions
    //causes the prince to slam down
    public void slam()
    {
        princeRigidBody.bodyType = RigidbodyType2D.Dynamic;
        princeRigidBody.AddForce(Vector2.down * 1000 * slamForce);
        activateHitbox(1);
    }

//Sword attack functions
    

//Sideways slam attack functions
    public void sideSlam()
    {
        princeRigidBody.bodyType = RigidbodyType2D.Dynamic;
        if (playerisRight)
        {
            princeRigidBody.AddForce(Vector2.down * 1000 * slamForce);
            princeRigidBody.AddForce(Vector2.left * 1000 * sideSlamForce);
        }
        else
        {
            princeRigidBody.AddForce(Vector2.down * 1000 * slamForce);
            princeRigidBody.AddForce(Vector2.right * 1000 * sideSlamForce);
        }
        activateHitbox(6);
    }

//Stab attack functions


//Spawn attack functions
    public void startSpawn()
    {
        chosenProjectile = Random.Range(0, numDiffProjectiles);
        isSpawning = true;
        spawnProjectile();
    }

    public void stopSpawn()
    {
        isSpawning = false;
    }

    public void spawnProjectile()
    {
        if (chosenProjectile == 0)
        {
            currentCooldown = cooldown;

            //Spawn Spears
            if (playerisRight)
            {
                Instantiate(spearProjectiles[0], projectileSpawnPoints[Random.Range(0, projectileSpawnPoints.Length)].position, Quaternion.identity);
            }
            else
            {
                Instantiate(spearProjectiles[1], projectileSpawnPoints[Random.Range(0, projectileSpawnPoints.Length)].position, Quaternion.identity);
            }
        }
        else
        {
            currentCooldown = cooldown * 10;
            int x = Random.Range(0, projectileSpawnPoints.Length);
            //Spawn 3 chackrams
            if (playerisRight)
            {
                Instantiate(chakramProjectiles[0], projectileSpawnPoints[x].position, Quaternion.identity);
                Instantiate(chakramProjectiles[1], projectileSpawnPoints[x].position, Quaternion.identity);
                Instantiate(chakramProjectiles[2], projectileSpawnPoints[x].position, Quaternion.identity);
            }
            else
            {
                Instantiate(chakramProjectiles[3], projectileSpawnPoints[x].position, Quaternion.identity);
                Instantiate(chakramProjectiles[4], projectileSpawnPoints[x].position, Quaternion.identity);
                Instantiate(chakramProjectiles[5], projectileSpawnPoints[x].position, Quaternion.identity);
            }

        }
    }

//Attack hitbox functions
    public void activateHitbox(int attackNumber)
    {
        attackHitboxes[attackNumber].SetActive(true);
    }

    public void deactivateHitbox(int attackNumber)
    {
        attackHitboxes[attackNumber].SetActive(false);
    }

//Attack end function
    public void attackEnd()
    {
        stateInt = State.ChoosingAttack;
    }
}

