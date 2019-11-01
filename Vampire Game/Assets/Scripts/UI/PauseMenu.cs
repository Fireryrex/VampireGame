using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    public static bool gameIsPaused = false;
    public static bool inInventory = false;

    public GameObject pauseMenuUI;
    public GameObject inventoryUI;


    // Update is called once per frame
    void Update()
    {
        //use whatever key to pause --> currently Escape
        if (Input.GetKey(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else if(gameIsPaused == false && inInventory == false)
            {
                Pause();
            }
        }
        else if (Input.GetKey(KeyCode.I)){
            Debug.Log("We pressed I.");
            Inventory();

        }
    }

    public void Resume(){
        Debug.Log("Resuming");
        pauseMenuUI.SetActive(false);
        inventoryUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    public void Resumen(){
        Debug.Log("Resuming");
        pauseMenuUI.SetActive(false);
        inventoryUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause(){
        Debug.Log("Pausing");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        //Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
    public void Inventory(){
        inventoryUI.SetActive(true);
    }
}
