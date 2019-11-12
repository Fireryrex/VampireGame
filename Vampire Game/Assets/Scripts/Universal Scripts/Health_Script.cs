using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public int health;
    public float knockback;
    private float coolDown;
    [SerializeField] float coolDownTime;
    public GameObject DeathAnimation;
    public float timeToDeath = 0;
    [SerializeField] Transform RespawnPoint;
    public int maxHealth;
    public string type;

    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0){
            coolDown -= Time.deltaTime;
            Debug.Log(coolDown);
        }
        else if (coolDown < 0){
            coolDown = 0;
        }
    }

    //Decreases the characters health by damage
    public void dealDamage(int damage)
    {
        if(coolDown == 0){
            coolDown = coolDownTime;
        particleDamageTrigger();
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
        //Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        
        //rb.AddForce(new Vector3(knockback, knockback, 0));
        }
    }

    public void heal(int healAmt)
    {
        health += healAmt;
    }

    public float getHealthPercent()
    {
        return (float)(health) / maxHealth;
    }

    public void setRespawnPoint(Transform rp)
    {
        RespawnPoint = rp;
    }

    public void respawn()
    {
        
        teleportPlayer(RespawnPoint);
        health = maxHealth;
    }

    private void particleDamageTrigger() {
        if (gameObject.GetComponent<Health_Script>().type == "BreakableObject"){
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
        }
        else if (gameObject.GetComponent<Health_Script>().type == "Enemy"){
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void teleportPlayer(Transform transformpoint)
    {
        transform.position = transformpoint.position;
    }
}