
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum states {Level1, Level2, Level3}; 
    public static GameManager instance;
    public int test = 100;
    public static GameObject player;
    public Vector2 RespawnPoint;
    public string RespawnScene;
    public int currentBlood;
    public string respawnLevel;
    public GameObject healthUI;
    private GameObject[] hearts = new GameObject[10];
    public int currentPlayerHealth;
     private void Awake() {
        instance = this;    
    }
    //Checks to see if a GameManager already exists and if so deletes any new copies. All the copies... Heh heh heh.
    private void Start() {
        //currentPlayerHealth = player.GetComponent<Health_Script>().health;
        currentBlood = 0;
        if(player == null){
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(GameObject.FindGameObjectsWithTag("GameManager").Length > 1 ){
            Debug.Log("Destroy");
            DestroyImmediate(this);
        }
        hearts = GameObject.FindGameObjectsWithTag("Hearts");  
        Debug.Log(hearts[0].tag);
    }
    private void Update() {
        checkForDead();
        if(player.GetComponent<Health_Script>().health < currentPlayerHealth ){
            emptyHeart();
        }
    }
    public void Respawn(){
        if(SceneManager.GetActiveScene().name != respawnLevel){
            SceneManager.LoadScene(respawnLevel);
            player.transform.position = RespawnPoint;
            player.GetComponent<Health_Script>().health = player.GetComponent<Health_Script>().maxHealth;
            FillHearts();

        }
        else{
            player.transform.position = RespawnPoint;
            player.GetComponent<Health_Script>().health = player.GetComponent<Health_Script>().maxHealth;
            FillHearts();
        }
    }
    public void checkForDead(){
        if (player.transform.position.y <= -51.0f){
            Respawn();
            FillHearts();
        }
        else if(player.GetComponent<Health_Script>().health <= 0){
            Respawn();
            FillHearts();
        }
    }
        public void emptyHeart(){
            Debug.Log("Good Morning");
            for (int i = player.GetComponent<Health_Script>().health; i < player.GetComponent<Health_Script>().maxHealth; i++){
                hearts[i].GetComponent<Image>().fillAmount = 0;
            }
    }
        public void FIllHeart(){
            
        }
        public void FillHearts(){
            for (int i = 0; i < player.GetComponent<Health_Script>().maxHealth; i++){
                hearts[i+1].GetComponent<Image>().fillAmount = 1;
            }
        }
}
