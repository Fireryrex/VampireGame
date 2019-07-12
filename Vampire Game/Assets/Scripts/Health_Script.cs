using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public int health;
    public float knockback;
    public GameObject DeathAnimation;
    public float timeToDeath = 0; 


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Decreases the characters health by damage
    public void dealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Am Dead");
            //Creates a game object that has an death animation
            //Object.Instantiate(DeathAnimation,transform);
            //destroy this game object
            Object.Destroy(gameObject,timeToDeath);
            
        }
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        
        //rb.AddForce(new Vector3(knockback, knockback, 0));
    }

    public void heal(int healAmt)
    {
        health += healAmt;
    }
}