
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    enum states {Level1, Level2, Level3}; 
    public static GameManager instance;
    public AudioManager AudioManagerInstance;
    public int test = 100;
    public static GameObject player;
    public Vector2 RespawnPoint;
    public int currentBlood;
    public string respawnLevel;
    public GameObject healthUI;
    private GameObject[] hearts = new GameObject[10];
    public int currentPlayerHealth;
    public GameObject FadeInQuad;
    public string id;
    [SerializeField] Cinemachine.CinemachineVirtualCamera gameCamera;

    public AsyncOperation sceneLoadOp = null;
    private AsyncOperation respawnSceneLoaded = null;
    public int sceneLoaded = 0;
    private bool respawning = false;
    private bool[] cutScenes = new bool[10];

     private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    //Checks to see if a GameManager already exists and if so deletes any new copies. All the copies... Heh heh heh.
    private void Start() {
        //currentPlayerHealth = player.GetComponent<Health_Script>().health;
        currentBlood = 0;
        AudioManagerInstance = GetComponent<AudioManager>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        hearts = GameObject.FindGameObjectsWithTag("Hearts");  
    }
    private void Update() {
        /*Debug.Log(respawnSceneLoaded == null);
        Debug.Log("(" + RespawnPoint.x + " , " + RespawnPoint.y);
        if (respawnSceneLoaded != null) //&& respawnSceneLoaded.isDone == true)
        {
            Debug.Log("asdlkljfhhalksdjfhaklsjdfhalskdjfh");
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            respawnSceneLoaded = null;
            Debug.Log("(" + RespawnPoint.x + " , " + RespawnPoint.y);
            player.transform.position = RespawnPoint;
        }*/
        checkForDead();
        if(player.GetComponent<Health_Script>().health < currentPlayerHealth)
        {
            emptyHeart();
        }
        if (sceneLoaded == -1 && sceneLoadOp.isDone == true)
        {
            sceneLoaded = 1;
        }
        
    }

    IEnumerator moveToRespawn(string respawnScene)
    {
        respawnSceneLoaded = SceneManager.LoadSceneAsync(respawnScene);
        yield return new WaitUntil(()=>respawnSceneLoaded.isDone);
        player.transform.position = RespawnPoint;
        player.GetComponent<Player_to_Crow>().findCrow();
        player.GetComponent<Player_to_Crow>().crowUpdatePosition();
        //player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    public void Respawn(string respawnScene, Vector2 respawnPosition){
        if(SceneManager.GetActiveScene().name != respawnScene){
            //Debug.Log("player is now repsawning");
            player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            //Debug.Log("Before respawning: " + (respawnSceneLoaded == null));
            
            StartCoroutine(moveToRespawn(respawnScene));
            StartCoroutine(freezePlayerForABit());
            //player.transform.position = respawnPosition;
            player.GetComponent<Health_Script>().health = player.GetComponent<Health_Script>().maxHealth;
            FillHearts();

        }
        else{
            //Debug.Log("respawnPoint Else");
            player.transform.position = respawnPosition;
            StartCoroutine(freezePlayerForABit());
            player.GetComponent<Player_to_Crow>().crowUpdatePosition();
            player.GetComponent<Health_Script>().health = player.GetComponent<Health_Script>().maxHealth;
            FillHearts();
        }
    }
    public void checkForDead(){
        if (player.transform.position.y <= -51.0f){
            Respawn(respawnLevel, RespawnPoint);
            FillHearts();
        }
        /*else if(player.GetComponent<Health_Script>().health <= 0){
            Respawn(respawnLevel, RespawnPoint);
            FillHearts();
        }*/
    }
        public void emptyHeart(){
            hearts = GameObject.FindGameObjectsWithTag("Hearts");
            for (int i = player.GetComponent<Health_Script>().health; i < player.GetComponent<Health_Script>().maxHealth; i++){
                hearts[i].GetComponent<Image>().fillAmount = 0;
            }
    }
        public void FIllHeart(){
            hearts = GameObject.FindGameObjectsWithTag("Hearts");
            player.GetComponent<Health_Script>().heal(1);
            hearts[player.GetComponent<Health_Script>().health-1].GetComponent<Image>().fillAmount = 100;
            
        }
        public void FillHearts(){
            for (int i = 0; i < player.GetComponent<Health_Script>().maxHealth; i++){
                hearts[i].GetComponent<Image>().fillAmount = 1;
            }
        }

        public void TransitionScene(string toScene,float TransitionTime, Vector2 moveTo)
        {
        try
        {
            StartCoroutine(TransitionToScene(toScene, TransitionTime, moveTo));
        }
        catch(MissingReferenceException e)
        {
            sceneLoadOp = SceneManager.LoadSceneAsync(toScene);
            sceneLoaded = -1;
        }
    }
    IEnumerator TransitionToScene(string toScene, float transitionTime, Vector2 moveTo)
    {

        bool halftime = false;
        float cutoff= 1;
        Time.timeScale = .01f;
        transitionTime *= .01f;
        for (float t = 0f ; t < transitionTime+.01f*.01f; t+=.02f*.01f) 
        {
            healthUI.GetComponent<Canvas>().enabled = false;
            if(t < transitionTime/2)
            {
                cutoff = Mathf.Lerp(1,0,t/(transitionTime/2));
            }
            else
            {
                if(halftime != true)
                {
                    halftime = true;
                    sceneLoadOp = SceneManager.LoadSceneAsync(toScene);
                    GameManager.instance.AudioManagerInstance.StopAll();
                    yield return new WaitUntil(() => sceneLoadOp.isDone);
                    GameManager.instance.AudioManagerInstance.Play(toScene);
                    player.transform.position = moveTo;
                    player.GetComponent<Player_to_Crow>().findCrow();
                    player.GetComponent<Player_to_Crow>().crowUpdatePosition();
                }
                cutoff = Mathf.Lerp(0,1, (t-transitionTime/2)/(transitionTime/2)  );
            }
            FadeInQuad.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff" , cutoff);
            yield return new WaitForSeconds(.02f *.01f);
        }
        healthUI.GetComponent<Canvas>().enabled = true;
        Time.timeScale = 1f;

    }

    IEnumerator freezePlayerForABit()
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public Cinemachine.CinemachineVirtualCamera returnCamera()
    {
        return gameCamera;
    }

    public bool getCutSceneStatus(int id){
        return cutScenes[id];
    }
}
