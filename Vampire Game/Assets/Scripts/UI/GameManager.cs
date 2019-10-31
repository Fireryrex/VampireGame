using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    enum states {Level1, Level2, Level3}; 
    public static GameManager instance;
    public List<Item> items = new List<Item>();
    public int test = 3;
    public static GameObject player;
    private bool reachedEnd = false;
     private void Awake() {
        instance = this;    
    }

    //Checks to see if a GameManager already exists and if so deletes any new copies. All the copies... Heh heh heh.
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        if(GameObject.FindGameObjectsWithTag("GameManager").Length > 1 ){
            Debug.Log("Destroy");
            DestroyImmediate(this);
        }
        DontDestroyOnLoad(this.gameObject);
        test += 1;   
    }
    private void Update() {
        if(reachedEnd == true){
            sceneTransition();
        }
    }
    private void sceneTransition(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void setReachedEnd(){
        reachedEnd = true;
    }

}
