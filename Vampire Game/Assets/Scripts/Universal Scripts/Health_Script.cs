using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public float health;
    public float knockback;
    public GameObject DeathAnimation;
    public float timeToDeath = 0;
    [SerializeField] Transform RespawnPoint;
    private float maxHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
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

            //Creates a game object that has an death animation
            //Object.Instantiate(DeathAnimation,transform);
            //destroy this game object

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //NEEDS WORK !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (this.tag == "Player")
            {
                respawn();
            }
            else
            {
                Object.Destroy(gameObject, timeToDeath);
            }
            
            
        }
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        
        //rb.AddForce(new Vector3(knockback, knockback, 0));
    }

    public void heal(int healAmt)
    {
        health += healAmt;
    }

    public float getHealthPercent()
    {
        return health / maxHealth;
    }

    public void setRespawnPoint(Transform rp)
    {
        RespawnPoint = rp;
    }

    public void respawn()
    {
        
        transform.position = RespawnPoint.position;
        health = maxHealth;
    }
}