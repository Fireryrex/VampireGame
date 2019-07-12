using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSwordEnemy_AI : MonoBehaviour
{
    private Transform target;
    public Transform left;
    public Transform right;
    public Transform stepBack;

    private float moveSpeed;
    public float regSpeed = 3f;
    public float chaseSpeed = 6f;
    public float stepBackSpeed = 1f;
    private bool isChasing = true;
    private bool isAttacking = false;
    private bool isJumping = false;
    private bool isSpearing = false;
    private bool isAggroed = false;
    public GameObject[] swordAttacks;
    private int attackNum = 0;
    private float startTime = 0f;
    private float cooldownTime = 50f;
    private float aggroTime = 0f;
    public float aggroDuration;
    private float attackLength = 0f;

    //jump attack variables
    public float jumpSpeed;
    public float chargeSpeed;
    private Rigidbody2D rb;
    private float targetX;              //player initial x
    private float targetY;              //player initial y
    public GameObject[] spearAttacks;
    public float jumpAttackCD;
    private bool isOnCD = false;

    //Debugging variables
    public GameObject swordCollider;
    public GameObject jumpCollider;
    public GameObject spearCollider;

    // Start is called before the first frame update
    void Start()
    {
        target = left;
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = regSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Cooldown stuff
        if(isOnCD)
        {
            cooldownTime += Time.deltaTime;
        }
        if(cooldownTime > jumpAttackCD)
        {
            isOnCD = false;
        }

        //back and forth stuff
        if (!isAttacking)
        {
            //this just flips the guy based on direction
            if (transform.position.x - target.transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.position.x - target.transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            //if it is moving left and arrives at left, it starts going to right
            if (target == left && transform.position.x == target.transform.position.x)
            {
                changeTarget(right);
            }

            //same as above but right;
            else if (target == right && transform.position.x == target.transform.position.x)
            {
                changeTarget(left);
            }
        }
        
        //jump logic
        if (isJumping && !isSpearing)
        {
            startTime += Time.deltaTime; //timer

            //activates at peak of the jump and charges to players initial position
            if(rb.velocity.y <= 0)
            { 
                this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(targetX, targetY, 0),
                    chargeSpeed * Time.deltaTime
                );
            }

            //first attack effect
            if (startTime > attackLength && attackNum == 0)
            {
                spearAttacks[0].SetActive(false);
                attackNum = 1;
                spearAttacks[1].SetActive(true);
                startTime = 0;
                attackLength = .5f;
            }

            //second attack effect
            if (startTime > attackLength && attackNum == 1)
            {
                spearAttacks[1].SetActive(false);
                attackNum = 2;
                isJumping = false;
                startCooldown(); //recombines with the attack logic
            }
        }

        //spear attack logic
        else if (isSpearing)
        {
            startTime += Time.deltaTime;            //timer

            if (startTime > attackLength && attackNum == 0)
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                spearAttacks[2].SetActive(false);
                attackNum = 1;
                nextAttack(2, .5f, 3);
            }

            else if (startTime > attackLength && attackNum == 1)
            {
                spearAttacks[3].SetActive(false);
                attackNum = 2;
                isSpearing = false;
                startCooldown();
            }
        }

        //attack logic
        else if (isAttacking)
        {
            startTime += Time.deltaTime; //attack timer

            //first attack effect
            if (startTime > attackLength && attackNum == 0)
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                swordAttacks[0].SetActive(false);
                attackNum = 1;
                //continueSwordAttacking();
                nextAttack(1, .5f, 1);
            }

            //2nd attack effect
            else if (startTime > attackLength && attackNum == 1)
            {
                this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                swordAttacks[1].SetActive(false);
                attackNum = 2;
                startCooldown();
            }

            //stepping back
            else if (attackNum == 2 && startTime < attackLength)
            {
                this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(stepBack.transform.position.x, stepBack.transform.position.y, 0),
                    stepBackSpeed * Time.deltaTime
                );
            }

            //back to chasing
            else if (startTime > attackLength && attackNum == 2)
            {
                stopAttacking();
            }
        }

        //chasing logic
        else if (isChasing && !isAttacking && !isSpearing)
        {
            if(isAggroed && aggroTime > aggroDuration)
            {
                deAggro();
            }
            else if(isAggroed)
            {
                aggroTime += Time.deltaTime;
            }
            this.gameObject.transform.position = Vector3.MoveTowards
                (
                    new Vector3(transform.position.x, transform.position.y, 0),
                    new Vector3(target.transform.position.x, target.transform.position.y, 0),
                    moveSpeed * Time.deltaTime
                );
        }        
    }
    
    //logic for activating specific attacks
    public void nextAttack(int weapon, float attackTime, int attackNumber)
    {
        /*for the int weapon:
            1 is the sword attack
            2 is the upwards spear attack
            3 is the jumping spear attack
        */
        if (weapon == 1 && !isSpearing)
        {
            jumpCollider.SetActive(false);                  //deactivates jump trigger to avoid wierdness
            spearCollider.SetActive(false);                 //same with spear collider
            swordCollider.SetActive(false);
            isAttacking = true;
            isSpearing = false;
            isJumping = false;
            swordAttacks[attackNumber].SetActive(true); //starts the sword attack
        }
        else if(weapon == 2)
        {
            jumpCollider.SetActive(false);                  //deactivates jump trigger to avoid wierdness
            spearCollider.SetActive(false);                 //same with spear collider
            swordCollider.SetActive(false);
            isSpearing = true;
            isAttacking = true;
            isJumping = false;
            spearAttacks[attackNumber].SetActive(true); //starts the spear attack
        }
        else if(weapon == 3)
        {
            if(!isOnCD)
            {
                jumpCollider.SetActive(false);                  //deactivates jump trigger to avoid wierdness
                spearCollider.SetActive(false);                 //same with spear collider
                swordCollider.SetActive(false);
                isAttacking = true;
                isJumping = true;
                isSpearing = false;
                targetX = target.transform.position.x;              //player initial x
                targetY = target.transform.position.y;              //player initial y
                rb.velocity = new Vector2(rb.velocity.x, 0);        //stops enemy from moving left and right    
                rb.AddForce(new Vector2(rb.velocity.x, jumpSpeed)); //adds a y force to "jump"
                cooldownTime = 0f;                                  //sets the jump attack cooldown to max
                isOnCD = true;                                      //starts cooldown timer
                //startJumpAttacking();                             //calls jump attack logic
                spearAttacks[attackNumber].SetActive(true);
            }
        }
        startTime = 0f;                                 //sets attack length
        aggroTime = 0f;                                 //resets aggro duration (may be unneeded)
        attackLength = attackTime;                      //sets attack length
    }

    //makes enemy start the step back phase
    public void startCooldown()
    {
        startTime = 0f;
        attackLength = 1.5f;
    }

    //makes enemy start chasing again and resets everything
    public void stopAttacking()
    {
        isAttacking = false;
        isJumping = false;
        isSpearing = false;
        attackNum = 0;
        swordCollider.SetActive(true);
        spearCollider.SetActive(true);
        jumpCollider.SetActive(true);
    }

    

    //changes the target to the parameter
    void changeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    //starts chasing the player if the player enters the collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            changeTarget(collision.gameObject.transform);
            moveSpeed = chaseSpeed;
            isAggroed = true;
            aggroTime = 0f;
            isChasing = true;
        }
    }

    //deaggros from the player and goes back to patrolling
    private void deAggro()
    {
        changeTarget(left);
        moveSpeed = regSpeed;
        isAttacking = false;
        isAggroed = false;
        isJumping = false;
        isChasing = true;
    }


    /*old code feel free to delete
    //first jump attack
    public void startJumpAttacking()
    {
        startTime = 0f;                         //similar to startSwordAttacking
        spearAttacks[0].SetActive(true);
        aggroTime = 0f;
        attackLength = .5f;
    }

    //second sword attack
    public void continueSwordAttacking()
    {
        startTime = 0f;                         //see startSwordAttacking
        swordAttacks[1].SetActive(true);
        attackLength = .5f;
    }

    //called by SpearSwordEnemy_Jump.cs to start jump logic
    public void jump()
    {
        if (!isJumping && !isOnCD)                              //makes sure the jump attack isnt in progress or on cd
        {
            swordCollider.SetActive(false);                     //deactivates the sword trigger to avoid wierdness
            spearCollider.SetActive(false);                     //same with spear collider
            isAttacking = true;
            isJumping = true;
            targetX = target.transform.position.x;              //player initial x
            targetY = target.transform.position.y;              //player initial y
            rb.velocity = new Vector2(rb.velocity.x, 0);        //stops enemy from moving left and right    
            rb.AddForce(new Vector2(rb.velocity.x, jumpSpeed)); //adds a y force to "jump"
            cooldownTime = 0f;                                  //sets the jump attack cooldown to max
            isOnCD = true;                                      //starts cooldown timer
            startJumpAttacking();                               //calls jump attack logic
        }
    }

    //first spear attack
    public void startSpearAttacking()
    {
        if(!isAttacking)
        {
            jumpCollider.SetActive(false);              //deactivate other attack triggers to avoid weirdness
            swordCollider.SetActive(false);
            isAttacking = true;
            isSpearing = true;
            startTime = 0f;
            aggroTime = 0f;
            spearAttacks[2].SetActive(true);
            attackLength = .5f;
        }
    }

    //first sword attack
    public void startSwordAttacking()
    {
        if (!isAttacking)                       //makes sure the sword attack isnt in progress
        {
            jumpCollider.SetActive(false);      //deactivates jump trigger to avoid wierdness
            spearCollider.SetActive(false);     //same with spear collider
            isAttacking = true;
            startTime = 0f;                     //sets attack length
            aggroTime = 0f;                     //resets aggro duration (may be unneeded)
            swordAttacks[0].SetActive(true);    //starts the sword attack
            attackLength = .5f;                 //sets attack length
        }
    }*/
}
