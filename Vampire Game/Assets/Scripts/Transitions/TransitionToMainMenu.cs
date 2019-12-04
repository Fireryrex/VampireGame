using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToMainMenu : MonoBehaviour
{
    void loadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
