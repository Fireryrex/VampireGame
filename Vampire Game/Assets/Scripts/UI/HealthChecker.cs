using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthChecker : MonoBehaviour
{
    public TMP_Text myText;
    public Health_Script playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
        myText.text = "Health: ";
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = "Health: " + playerHealth.health;
    }
}
