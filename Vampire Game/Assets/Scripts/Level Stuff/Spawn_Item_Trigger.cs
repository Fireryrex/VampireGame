using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Item_Trigger : MonoBehaviour
{
    private Spawn_Enemy_Or_Object Spawn_Script;
    private bool onCooldown = false;
    private float time = 0f;
    [SerializeField] float Cooldown;
    // Start is called before the first frame update
    void Start()
    {
        Spawn_Script = GetComponentInParent<Spawn_Enemy_Or_Object>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        if(onCooldown)               //Cooldown counter
        {
            time += Time.deltaTime;
        }
        if(time >= Cooldown)
        {
            onCooldown = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !onCooldown)   //only the player can trigger if the spawn is off cooldown
        {
            Spawn_Script.SpawnItem();                       //calls the spawn function of the spawnscript of the parent 
            time = 0f;                                      //resets cooldown and activates it
            onCooldown = true;
        }
    }
}
