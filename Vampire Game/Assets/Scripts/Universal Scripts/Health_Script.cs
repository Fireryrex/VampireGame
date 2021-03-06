﻿using UnityEngine;
using UnityEngine.UI;


public class Health_Script : MonoBehaviour
{
    public int health;
    public float knockback;
    protected float coolDown;
    [SerializeField] protected float coolDownTime;
    public GameObject DeathAnimation;
    public float timeToDeath = 0;
    public float currentBlood;
    [SerializeField] Transform RespawnPoint;
    public int maxHealth;
    public string type;
    [SerializeField] protected bool ignoreDeathfield = false;
    [SerializeField] protected GameObject bloodParticleSystem;

    public string respawnPoint;
    public Vector2 respawnPosition;

    private Animator playerAnim;
    public string damagedSound = " ";

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        ResetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDown > 0){
            coolDown -= Time.deltaTime;
        }
        else if (coolDown < 0){
            coolDown = 0;
        }
        if(tag == "Player"){
            newHeart();
            updateVial();
        }
    }
    public void OnParticleCollision(GameObject other){
         if(other.CompareTag("Player")){

            Debug.Log("Player was touched");
            other.GetComponent<Health_Script>().currentBlood += 1;
         
         }
    }
    public void newHeart(){
        if (currentBlood >= 100 && health < maxHealth){
            GameManager.instance.FIllHeart();
            currentBlood = 0;
        }
        else if(currentBlood > 100){
            currentBlood = 100;
        }
    }
    public void updateVial(){
        GameObject.Find("VialBlood").GetComponent<Image>().fillAmount = currentBlood/100;
    }

    //Decreases the characters health by damage
    public virtual void dealDamage(int damage)
    {
        if(coolDown == 0)
        {
            GameManager.instance.AudioManagerInstance.Play(damagedSound);
            coolDown = coolDownTime;
            //particleDamageTrigger();
            health -= damage;
            Instantiate(bloodParticleSystem, gameObject.transform.position, Quaternion.identity);
                
            if (health <= 0)
            {
                if (this.tag == "Player")
                {
                    playerAnim.SetBool("Respawn", true);
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    GameManager.instance.Respawn(respawnPoint, respawnPosition);
                    playerAnim.SetBool("Respawn", false);
                    gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    GameManager.instance.returnCamera().m_Follow = GetComponentInChildren<CameraReturnPoint>().returnThisTransform();
                    GameManager.instance.returnCamera().m_Lens.OrthographicSize = 10;
                }
                else
                {
                    Object.Destroy(gameObject, timeToDeath);
                }
            }
            if (this.tag == "Player")
            {
                playerAnim.SetBool("Hurt", true);
                playerAnim.SetBool("Moving", false);
                playerAnim.SetBool("Attack1", false);
                playerAnim.SetBool("Attack2", false);
                playerAnim.SetBool("Attack3", false);
                playerAnim.SetBool("Jump", false);
                GameManager.instance.emptyHeart();
            }
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

    public void teleportPlayer(Transform transformpoint)
    {
        transform.position = transformpoint.position;
    }
    public bool getDeathFieldVariable()
    {
        return ignoreDeathfield;
    }
    public virtual void ResetHealth()
    {
        health = maxHealth;
    }

    public void accessNotTakingDamage()
    {
        playerAnim.SetBool("Hurt", false);
    }

    public void respawnAnimPlay()
    {
        playerAnim.SetBool("Respawn", true);
    }

    public void respawnAnimEnd()
    {
        playerAnim.SetBool("Respawn", false);
    }
}