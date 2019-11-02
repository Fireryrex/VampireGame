using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a base class that can be extended for other melee attacks that do specific things to enemies.
public class Attack : MonoBehaviour
{

    //i can check the windows withiin Attack and implement cancellable as a method
    public float attackCancelStart;//this refers to the time before which it is cancellable by dashing or jumping
    public float attackDuration; //this refers to the overall duration of the attack,
    public float attackCancelEnd;//this refers to the time after which it is cancellable

    public float queueInputAfter;// tells the statemachine when it should start accepting inputs for the next attack. 

    //these are mutually exclusive
    public bool canwalk; //can we still walk while attacking? 
    public bool airborne; //is this an aerial attack? If so it should cancel no matter what when you hit the ground. also determines which movement function we use.;
    public bool movesPlayer; //this attack moves the player on its own.

    public Vector2 attackDrag; //will reduce movement speed by dimension when used. 0 = completely cancel velocity in that dimension.


    public AnimationCurve animationCurveX; //for velocity during attack. 
    public AnimationCurve animationCurveY; //for velocity during attack.
    public AnimationCurve animationCurveT; //for velocity during attack. scales from 0 to 1.

    public int[] nextAttackInCombo; // -1 if final, is a function

    protected float startTime;
    [SerializeField] protected float knockback;
    [SerializeField] protected int damage;
    [SerializeField] protected LayerMask damages;


    virtual public int GetNextAttackInCombo()
    {
        //default will just get the first.
        if(nextAttackInCombo.Length>0)
            return nextAttackInCombo[0];
        return -1;
    }

    
    virtual public Vector2 getVel()//reads animationCurves and outputs the moveVec. normalized to 1
    {
        return new Vector2(animationCurveX.Evaluate(animationCurveT.Evaluate( (Time.time-startTime) / attackDuration )) ,
         animationCurveY.Evaluate(animationCurveT.Evaluate( (Time.time-startTime) / attackDuration )   ));
    }

    virtual public bool cancelable() // a method
    {
        float elapsedTime = Time.time - startTime;
        if( elapsedTime < attackCancelStart || elapsedTime > attackCancelEnd)
            return true;
        else
            return false;
    }

    virtual public void OnEnable()
    {
        startTime= Time.time;
        //if theres an animation play it.
        //other than enabling the game object the controller doesnt do anything. it all has to be done here.
    }

    public virtual void OnTriggerEnter2D (Collider2D collider)
    {
        if( (collider.gameObject.layer & damages) != 0)
        {
            collider.gameObject.GetComponent<Health_Script>().dealDamage(damage); // would like to be able to pass it knockback as well.
        }
    }

}
