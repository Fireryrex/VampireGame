using UnityEngine;
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

    // Start is called before the first frame update
    void Start()
    {
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
        if (currentBlood > 100 && health < maxHealth){
            GameManager.instance.FIllHeart();
        }
        else if(currentBlood > 100){
            currentBlood = 100;
        }
    }
    public void updateVial(){
        Debug.Log(currentBlood/100);
        GameObject.Find("VialBlood").GetComponent<Image>().fillAmount = currentBlood/100;
    }

    //Decreases the characters health by damage
    public virtual void dealDamage(int damage)
    {
        if(coolDown == 0){
            coolDown = coolDownTime;
        //particleDamageTrigger();
        health -= damage;
        Instantiate(bloodParticleSystem, gameObject.transform.position, Quaternion.identity);
                
        if (health <= 0)
        {
            if (this.tag == "Player")
            {
                respawn();
            }
            else
            {
                Object.Destroy(gameObject, timeToDeath);
            }
        }
        if (this.tag == "Player"){
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

    public void setRespawnPoint(Transform rp)
    {
        RespawnPoint = rp;
    }

    public void respawn()
    {
        
        teleportPlayer(RespawnPoint);
        health = maxHealth;
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
}