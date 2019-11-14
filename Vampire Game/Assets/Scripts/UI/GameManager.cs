using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum states {Level1, Level2, Level3}; 
    public static GameManager instance;
    public List<Item> items = new List<Item>();
    public int test = 0;
    public static GameObject player;
    public Vector2 RespawnPoint;
    public int currentBlood;
    private bool reachedEnd = false;
    public string respawnLevel;
     private void Awake() {
        instance = this;    
    }

    //Checks to see if a GameManager already exists and if so deletes any new copies. All the copies... Heh heh heh.
    private void Start() {
        currentBlood = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        if(GameObject.FindGameObjectsWithTag("GameManager").Length > 1 ){
            Debug.Log("Destroy");
            DestroyImmediate(this);
        }
           
    }
    private void Update() {
        if(reachedEnd == true){
            sceneTransition();
        }
        checkForDead();
    }
    private void sceneTransition(){
        test += 1;
        reachedEnd = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        player.transform.position = RespawnPoint;
    }
    public void setReachedEnd(){
        reachedEnd = true;
    }
    public void Respawn(){
        player.transform.position = RespawnPoint;
        player.GetComponent<Health_Script>().health = player.GetComponent<Health_Script>().maxHealth;
    }

    public void checkForDead(){
        if (player.transform.position.y <= -51.0f){
            Respawn();
        }
        else if(player.GetComponent<Health_Script>().health <= 0){
            Respawn();
        }
    }
}
