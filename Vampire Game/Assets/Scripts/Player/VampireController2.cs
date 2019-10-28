using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VampireController2 : MonoBehaviour
{
    Dictionary<string,Action> States;
    
    [SerializeField]
    string state;//for debugging, dont set

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

    [Range(.1f,100f)]
    public float dashDistance;
    [Range(.01f,60f)]
    public float dashTime;
    public Vector2 dashHurtboxBounds;
    public Vector2 dashHurtboxOffset;

    public bool HasWeapon;
    public bool HasDoubleJump;
    public bool HasDash;

    void Awake()
    {
        States = new Dictionary<string,Action>();
        rb = GetComponent<Rigidbody2D>();
        state="DefaultState";
        States["DefaultState"] = DefaultState;
        States["JumpState"]  = JumpState;
        States["FallingState"] = FallingState;
        currentGravity = gravityScale;
        colliders = new Collider2D[8];
        currentJumps = jumps;
    }

    void Start()
    {
        
    }

    void FixedUpdate()//set max allowable timestep to 1/60, same as normal fixed timestep.
    {
        if(moveVec!=Vector2.zero)
            rb.MovePosition(rb.position + moveVec);
    }

    void Update()
    {
        States[state]();
    }

    void EnterDefaultState()
    {
        state= "DefaultState";
        currentJumps  = jumps;
        DefaultState();
    }

    //moveVec, grounded
    void DefaultState()
    {
        SetMoveDir();
        CheckGrounded();
        CheckRoofed();
        ApplyGravity();


        if(Input.GetKeyDown(KeyCode.Space) & jumpCooldown < (Time.time - lastJumpTime) & !roofed & grounded)
        {
            ExitDefaultState();
            EnterJumpState();
        }
        else if(!grounded)
        {
            ExitDefaultState();
            EnterFallingState();
        }
    }

    void ExitDefaultState()
    {
        //moveVec = Vector2.zero;
    }

    bool stillHoldingSpace;
    void EnterJumpState() //should I segregate between jumping and falling? I think so.
    {
        grounded = false;
        stillHoldingSpace = Input.GetKey(KeyCode.Space);
        state = "JumpState";
        lastJumpTime = Time.time;
        moveVec.y = Mathf.Sqrt(2f * gravityScale * jumpHeight * Time.fixedDeltaTime);
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
        if(moveVec.y <= 0 || roofed)
        {
            ExitJumpState();
            EnterFallingState();
        }
        if(grounded)
        {
            ExitJumpState();
            EnterDefaultState();
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
        }
        if(currentJumps>0 && HasDoubleJump)


    }

    void ExitFallingState()
    {

    }

    void EnterDashingState()
    {

    }

    void DashingState()
    {

    }

    void ExitDashingState()
    {
        
    }

    void EnterRecoilState()
    {

    }

    void RecoilState()
    {

    }

    void ExitRecoilState()
    {

    }

    void EnterStandingAttackingState()
    {

    }

    void StandingAttackingState()
    {

    }

    void ExitStandingAttackingState()
    {

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
    }

    //applies input to moveVec without resetting Y. takes air control into account. 
    void SetMoveDirAir()
    {
        float apply = ((Input.GetKey(KeyCode.A) ? -1 : 0) +
            (Input.GetKey(KeyCode.D) ? 1 : 0))*walkSpeed*Time.fixedDeltaTime*airControl;
        if(  FloatComp( Mathf.Abs(moveVec.x) , Mathf.Abs(maxAirSpeed * Time.fixedDeltaTime)  ) < 0)
            {moveVec.x += apply;}
        else if( FloatComp(  moveVec.x , maxAirSpeed*Time.fixedDeltaTime) >= 0  && apply < 0) //allow decelleration but not accelleration above max. 
            {moveVec.x += apply; }
        else if( FloatComp ( moveVec.x , -maxAirSpeed*Time.fixedDeltaTime) <= 0 && apply > 0)
            {moveVec.x += apply;}
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
