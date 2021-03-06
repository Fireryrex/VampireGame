﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VampireController2 : MonoBehaviour
{
    Dictionary<string,Action> States;
    
    [SerializeField]
    string state;//for debugging, dont set
    [SerializeField] bool unlockedAttack = false;
    [SerializeField] bool unlockedAttack2 = false;
    [SerializeField] bool unlockedAttack3 = false;
    [SerializeField] bool unlockedDash = false;

    public float walkSpeed;
    public float gravityScale;
    public bool gravityOn;
    public bool grounded;
    public float GroundCheckOffset;
    public Vector2 GroundCheckBounds;
    public LayerMask ground;

    public float RoofCheckOffset;
    public Vector2 RoofCheckBounds;
    public LayerMask roof;
    [Range(.01f,1f)]
    public float jumpFloatiness;
    float currentGravity;

    public float airControl;
    public int jumps;
    public float jumpCooldown;
    public float fallSpeedCap;
    public float jumpHeight;
    bool useHeight = true;
    public float maxAirSpeed;
    float lastJumpTime;
    int currentJumps;
    
    Collider2D[] colliders;
    Rigidbody2D rb;
    Vector2 moveVec; //changed by frame

    public bool roofed;

    public KeyCode dashButton;
    [Range(.1f,100f)]
    public float dashDistance;
    [Range(.01f,60f)]
    public float dashTime;
    public Vector2 dashHurtboxBounds; // if you want the player to have a different hitbox while dashing
    public Vector2 dashHurtboxOffset; // not implemented rn
    public float dashCooldown;
    public bool resetAirdashOnJump;
    bool airDash;
    float dashStartTime;
    Vector2 dashX;

    public GameObject attacks;
    public float comboTimeout;
    float lastAttackEndTime;


    //public float invincibilityTime;
    public float knockbackDistance;
    public float damagedDuration;
    bool damaged;//used to exit states when the player takes damage.
    float damagedTime;

    bool facingRight;
    

    public bool HasWeapon;
    public bool HasDoubleJump;
    public bool HasDash;

    //Attack indexes
    [SerializeField] int groundAttackIndex;
    [SerializeField] int airAttackIndex;
    [SerializeField] int fallAttackIndex;

    public Vector2 lastPlayerPosition;

    private Animator playerAnim;

    void Awake()
    {
        States = new Dictionary<string,Action>();
        rb = GetComponent<Rigidbody2D>();
        state="DefaultState";

        States["DefaultState"] = DefaultState;
        States["JumpState"]  = JumpState;
        States["FallingState"] = FallingState;
        States["DashingState"] = DashingState;
        States["AttackingState"] = AttackingState;
        currentGravity = gravityScale;
        colliders = new Collider2D[8];
        currentJumps = jumps;
        facingRight = transform.localScale.x>0;
        playerAnim = GetComponent<Animator>();
    }

    void Start()
    {
    
    }

    void FixedUpdate()//set max allowable timestep to 1/60, same as normal fixed timestep.
    {
        if(moveVec!=Vector2.zero)
        {
            rb.MovePosition(rb.position + moveVec);
            if(FloatComp(moveVec.x,0,.001f) != 0f)
                facingRight = moveVec.x>0;
        }
    }

    void Update()
    {
        States[state]();
        transform.localScale = new Vector3(facingRight? 1 : -1 , 1,1);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            playerAnim.SetBool("Moving", true);
        }
        else
        {
            playerAnim.SetBool("Moving", false);
        }
    }
    public void unlockAttack(){
        unlockedAttack = true;
    }
    public void unlockAttack2(){
        unlockedAttack2 = true;
    }
    public void unlockAttack3(){
        unlockedAttack3 = true;
    }
    public void unlockDash(){
        unlockedDash = true;
    }
    public void unlockDoubleJump(){
        HasDoubleJump = true;
    }

    void TakeDamage()
    {
        damaged = true;
        damagedTime = Time.deltaTime;
        GameManager.instance.AudioManagerInstance.Play("player_hit");
    }

    void EnterDamagedState()
    {
        state="DamagedState";
    }

    void DamagedState()
    {
        moveVec = knockbackDistance/damagedDuration*Time.fixedDeltaTime*Vector2.left * transform.localScale.x; //if facing right using transform, go left.
        if(Time.time-damagedTime > damagedDuration)
        {
            ExitDamagedState();
            EnterFallingState();//may happen in the air so go to falling state instead of default.
        }
    }

    void ExitDamagedState()
    {
        damaged = false;
    }

    void EnterDefaultState()
    {
        state= "DefaultState";
        currentJumps  = jumps;
        airDash = true;
        DefaultState();
    }

    //moveVec, grounded
    void DefaultState()
    {
        SetMoveDir();
        CheckGrounded();
        CheckRoofed();
        ApplyGravity();
        lastPlayerPosition = transform.position;
        if (facingRight == true){
            lastPlayerPosition = lastPlayerPosition + new Vector2(-1.0f, 0.0f);
        }
        else{
            lastPlayerPosition = lastPlayerPosition + new Vector2(1.0f, 0.0f);
        }
        

        if(damaged)
        {
            ExitDefaultState();
            EnterDamagedState();
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space) & jumpCooldown < (Time.time - lastJumpTime) & !roofed & grounded)
        {
            ExitDefaultState();
            EnterJumpState();
            return;
        }
        if(!grounded)
        {
            
            ExitDefaultState();
            EnterFallingState();
            return;
        }
        if (Input.GetKeyDown(dashButton) &&
            (Time.time-dashStartTime) > dashCooldown &&
                 FloatComp( moveVec.x, 0 , .03f)!= 0 && unlockedDash == true)
        {
            ExitFallingState();
            EnterDashingState();
            return;
        }
        if(Input.GetKeyDown(attackButton) && unlockedAttack )
        {
            ExitDefaultState();
            //maybe instead of having 1 previous reserve attack, i should handle stuff on a combo by combo basis. 
            //like an array of reserve attacks by combo and times.
            //well i guess 1 combo shouldnt be intermixable with another... so maybe just having 1 is fine... but maybe i definitely should store it as a 
            //"combo" object instead of just an int. 
            if(Input.GetKey(KeyCode.W))
                {
                attacknum = 6;
                grounded = false;
                }
            else
                attacknum =   (Time.time- Mathf.Max(lastAttackEndTime,attackStartTime) > comboTimeout || reserveAttackNum<0) ? groundAttackIndex : reserveAttackNum ; //this is what attacks
            EnterAttackingState();
            return;
        }
    }

    void ExitDefaultState()
    {
        //moveVec = Vector2.zero;
    }

    bool stillHoldingSpace;
    void EnterJumpState() //should I segregate between jumping and falling? I think so.
    {
        playerAnim.SetBool("Jump", true);
        if(currentJumps == 0)
        {
        lastPlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
        grounded = false;
        stillHoldingSpace = Input.GetKey(KeyCode.Space);
        state = "JumpState";
        lastJumpTime = Time.time;
        moveVec.y = Mathf.Sqrt(2f * gravityScale * jumpHeight * Time.fixedDeltaTime);
        currentJumps --;
        GameManager.instance.AudioManagerInstance.Play("player_jump");
        if(resetAirdashOnJump)
            airDash = true;
        JumpState();
    }

    void JumpState()
    {
        if(Input.GetKeyUp(KeyCode.Space))
            stillHoldingSpace = false;
        SetGravity(stillHoldingSpace ? gravityScale*jumpFloatiness : gravityScale);

        if(Time.time - lastJumpTime>jumpCooldown)
            CheckGrounded();

        SetMoveDirAir();
        ApplyGravity();
        CheckRoofed();

        if(damaged)
        {
            ExitJumpState();
            EnterDamagedState();
            return;
        }
        if(moveVec.y <= 0 || roofed)
        {
            ExitJumpState();
            EnterFallingState();
            return;
        }
        if(grounded)
        {
            ExitJumpState();
            EnterDefaultState();
            return;
        }
        if(airDash & Input.GetKey(dashButton) &&
            (Time.time - dashStartTime > dashCooldown) &&
            (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && unlockedDash)    )
        {
            airDash = false;
            ExitJumpState();
            EnterDashingState();
            return;
        }
        if(Input.GetKeyDown(attackButton) && unlockedAttack)
        {
            if(Input.GetKey(KeyCode.S) && unlockedAttack)
                attacknum = fallAttackIndex ; //this is what attacks. combos dont carry over in air i guess. or its a single 
            else
                attacknum = airAttackIndex;
            EnterAttackingState();

            return;
        }
        if(currentJumps>0 && HasDoubleJump && Input.GetKeyDown(KeyCode.Space)  && (Time.time - lastJumpTime>jumpCooldown) )
        {
            ExitFallingState();
            EnterJumpState();
            return;
        }
        
    }

    void ExitJumpState()
    {
        SetGravity(gravityScale);
    }

    void EnterFallingState()
    {
        SetGravity(gravityScale);
        moveVec.y = 0;
        state = "FallingState";
    }

    void FallingState()
    {
        CheckGrounded();
        SetMoveDirAir();
        ApplyGravity();
        if(grounded)
        {
            ExitFallingState();
            EnterDefaultState();
            return;
        }
        if(damaged)
        {
            ExitFallingState();
            EnterDamagedState();
            return;
        }
        if(currentJumps>0 && HasDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            ExitFallingState();
            EnterJumpState();
            return;
        }
        if(airDash & Input.GetKey(dashButton) &&
            (Time.time - dashStartTime > dashCooldown) &&
            (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))    )
        {
            airDash = false;
            ExitFallingState();
            EnterDashingState();
            return;
        }
        if(Input.GetKeyDown(attackButton) && unlockedAttack)
        {
            if(Input.GetKey(KeyCode.S) && unlockedAttack)
                attacknum = fallAttackIndex ; //this is what attacks. combos dont carry over in air i guess. or its a single 
            else
                attacknum = airAttackIndex;
            EnterAttackingState();
            return;
        }

    }

    void ExitFallingState()
    {
        playerAnim.SetBool("Jump", false);
    }

    void EnterDashingState()
    {
        if(unlockedDash == true){
        dashStartTime = Time.time;
        state = "DashingState";
        SetMoveDir();
        dashX = moveVec.normalized; //get left or right
            GameManager.instance.AudioManagerInstance.Play("player_dash");
        //change colliders
        DashingState();
        }
    }

    void DashingState()
    {
        //dashing state can exit to an attack.
        moveVec = dashX * dashDistance/dashTime*Time.fixedDeltaTime;
        if(Time.time- dashStartTime > dashTime)
        {
            ExitDashingState();
            EnterFallingState();
        } 
        if(damaged)
        {
            ExitDefaultState();
            EnterDamagedState();
            return;
        }
        if(Input.GetKeyDown(attackButton))
        {
            moveVec = dashX* dashDistance*(1-(Time.time-dashStartTime)/dashTime);
            FixedUpdate();
            ExitDashingState();
            attacknum = 0;
            EnterAttackingState();
        }
    }

    void ExitDashingState()
    {
        SetMoveDir();
    }



    /*standing attack
    //maybe i can have a child of the player
    //called attacks, the state number corresponds to what gets activated.
    each attack needs properties : damage,various times,a hitbox, what follows,a movement,interruptable...
    */

    //extra ideas : a window during which hitting the attack again skips the next attacks startup lag. 
    int attacknum;
    float attackStartTime;
    Attack currentAttack;
    public KeyCode attackButton;
    Transform attack;
    int reserveAttackNum; //used to allow late presses by a bit.
    bool attackQueued = false;


    //attack properties



    void EnterAttackingState() 
    {
        attack = attacks.transform.GetChild(attacknum);
        attack.gameObject.SetActive(true);
        //Debug.Log("Attack set to true");
        state = "AttackingState";
        currentAttack = attack.gameObject.GetComponent<Attack>();
        attackStartTime = Time.time;
        attacknum = -1;
        attackQueued = false;
        moveVec *= currentAttack.attackDrag;
        GameManager.instance.AudioManagerInstance.Play("player_atk");
    }
    //maybe i should implement a "cooldown" instead of immediately setting attacknum to -1 or whatever when it exits - like let it go a little longer.
    void AttackingState() // attacking state is pretty messy cuz it has to be able to accomodate so many different types of attacks.
    {
        if(currentAttack.movesPlayer) //this is my "in attack" movement 
        {
            moveVec = currentAttack.getVel();
            moveVec*=transform.localScale;
        }
        //player controller still has control
        if(currentAttack.airborne) //if the attack is an aerial
        {
            ApplyGravity();
            moveVec*=currentAttack.attackDrag;
            CheckGrounded();
            CheckRoofed();
            if(roofed)//make sure we dont stick to the roof if we hit it. 
            {
                while(moveVec.y > 0)
                    ApplyGravity();
            }
/*            if(grounded) //quit the attack if we hit the ground                   Victor: Don't think we need to do this
            {
                ExitAttackingState();
                EnterDefaultState();
                return;
            }
*/            
        }
        if(currentAttack.canwalk) //if the attacks not an aerial and it lets us walk, walk. 
        {
            SetMoveDir();
            CheckGrounded();
            CheckRoofed();
            ApplyGravity();
            moveVec *= currentAttack.attackDrag;
        }
        

        if(currentAttack.cancelable()) //is the attack cancellable this frame
        {
            if(Input.GetKeyDown(KeyCode.Space) )//try jumping to cancel
            {
                if (  jumpCooldown < (Time.time - lastJumpTime) && !roofed && grounded  )//groundjump
                {
                    ExitAttackingState();
                    EnterJumpState();
                    return;
                }
                else if( currentJumps>0 && HasDoubleJump&& !roofed )//airjump
                {
                    ExitAttackingState();
                    EnterJumpState();
                    return;
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftShift)) // try dashing to cancel
            {
                ExitAttackingState();
                return;
            }
        }

        if(Time.time - attackStartTime > currentAttack.queueInputAfter && Input.GetKeyDown(attackButton)) //start queueing attacks at this point
        {
            attacknum = currentAttack.GetNextAttackInCombo();
            attackQueued = true;
        }
        if(Time.time- attackStartTime > currentAttack.attackDuration) // attack has ended, proceed to next state. 
        {
            if(!attackQueued || attacknum ==-1) //done attacking (attacknum set to -1 in enter attacking state)
            {
                ExitAttackingState();
                EnterFallingState();
            }
            else // attacknum was set to something, proceed to do that.
            {
                attack.gameObject.SetActive(false);
                EnterAttackingState();
            }
        }
        if(damaged)
        {
            ExitAttackingState();
            reserveAttackNum = -1;
            EnterDamagedState();
            return;
        }
    }

    void ExitAttackingState()
    {    
        reserveAttackNum = currentAttack.GetNextAttackInCombo();
        currentAttack.end_Attack();
        currentAttack.gameObject.SetActive(false);
        lastAttackEndTime = Time.time;
        attacknum = -1;
    }


    public void access_Attack_Activation()
    {
        currentAttack.activate_Attack_Hitbox();
    }

    public void access_Attack_Deactivation()
    {
        currentAttack.deactivate_Attack_Hitbox();
    }

    public void access_End_Attack()
    {
        currentAttack.end_Attack();
    }


    //need grounded attack, air attack, maybe a family of attacking states.
    //need double jump or N jump
    //need knockback state.
    //need wall grappled state

    //FUNCTIONS


    //moveVec
    void ZeroMoveDir()
    {
        moveVec= Vector2.zero;
    }

    //moveVec
    void SetMoveDir()
    {
        moveVec = ((Input.GetKey(KeyCode.A) ? Vector2.left : Vector2.zero) +
         (Input.GetKey(KeyCode.D) ? Vector2.right : Vector2.zero)).normalized *walkSpeed*Time.fixedDeltaTime;
        //if (GameManager.instance.AudioManagerInstance.IsPlaying("player_step") && moveVec == Vector2.zero)
        //{
        //    GameManager.instance.AudioManagerInstance.Stop("player_step");
        //}
        //if(!GameManager.instance.AudioManagerInstance.IsPlaying("player_step") && moveVec != Vector2.zero)
        //{
        //    GameManager.instance.AudioManagerInstance.Play("player_step");
        //}
    }

    //applies input to moveVec without resetting Y. takes air control into account. 
    void SetMoveDirAir()
    {

        //overrided air control
        float y = moveVec.y;
        SetMoveDir();
        moveVec *= airControl;
        moveVec.y = y;
       /*
        float apply = ((Input.GetKey(KeyCode.A) ? -1 : 0) +
            (Input.GetKey(KeyCode.D) ? 1 : 0))*walkSpeed*Time.fixedDeltaTime*airControl;
        if(  FloatComp( Mathf.Abs(moveVec.x) , Mathf.Abs(maxAirSpeed * Time.fixedDeltaTime)  ) < 0)
            {moveVec.x += apply;}
        else if( FloatComp(  moveVec.x , maxAirSpeed*Time.fixedDeltaTime) >= 0  && apply < 0) //allow decelleration but not accelleration above max. 
            {moveVec.x += apply; }
        else if( FloatComp ( moveVec.x , -maxAirSpeed*Time.fixedDeltaTime) <= 0 && apply > 0)
            {moveVec.x += apply;}*/
    }

    //moveVec
    void ApplyGravity() //rn gravity is linear
    {
        if(currentGravity == 0 || gravityOn ==false )
            print("Apply Gravity being called even though scale is " +
                gravityScale + " and gravity On is " + gravityOn);
        if(gravityOn && moveVec.y >= -fallSpeedCap*Time.fixedDeltaTime)
            moveVec += Vector2.down * currentGravity * Time.fixedDeltaTime;
    }

    //grounded
    void CheckGrounded()
    {
        grounded = CheckBoxOverlap(transform.position + Vector3.up*GroundCheckOffset, GroundCheckBounds, 0f, ground);
        if(grounded)
        {
            playerAnim.SetBool("Jump", false);
        }
    }

    //roofed
    void CheckRoofed()
    {
        roofed = CheckBoxOverlap( transform.position + Vector3.up*RoofCheckOffset, RoofCheckBounds, 0f, roof);
    }

    bool CheckBoxOverlap( Vector2 center, Vector2 size, float rotation, LayerMask checkLayers)
    {
        ContactFilter2D c = new ContactFilter2D();
        c.layerMask = roof;
        c.useLayerMask = true;
        return Physics2D.OverlapBox(center,size, rotation,c,colliders) > 0;
    }

    //gravityScale , lastGravity
    void GravityOn()
    {
        currentGravity = gravityScale;
    }

    //gravityScale, lastGravity
    void GravityOff()
    {
        currentGravity = 0;
    }

    //gravityScale, lastGravity
    void SetGravity(float g)
    {
        if(g!=currentGravity)
        {
            currentGravity = g;
        }
    }

    void ResetGravity()
    {
        currentGravity = gravityScale;
    }

    float FloatComp(float a, float b, float tolerance = .000001f)
    {
        if(float.IsNaN(a) || float.IsNaN(b))
            return float.NaN;
        else if(float.IsInfinity(a) && float.IsInfinity(b))
            return float.PositiveInfinity;
        else if( Mathf.Abs(Mathf.Abs(a) - Mathf.Abs(b)) < tolerance)
            return 0f;
        else if( a - b > tolerance)
            return 1f;
        else if (b-a > tolerance)
            return -1f;
        else
            return float.NaN;

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + Vector3.up*GroundCheckOffset , GroundCheckBounds);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up*RoofCheckOffset , RoofCheckBounds);
    }
}

/*
 * Make character not push things through walls
 * match jump of other prefab
 * create a shorthop and longhop based on jump button hold time
 * essentially make this char match with other one
 */

    
