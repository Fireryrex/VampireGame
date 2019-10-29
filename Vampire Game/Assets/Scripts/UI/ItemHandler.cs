using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Item item;
    public Transform respawn;
    public int maxHealth;
    private int playerHealth;
    

    void Pickup()
    {
        InventoryUI.instance.Add(item);
        Debug.Log(item.name + " was pciked up.");
        Destroy(gameObject);
    }
    void gameOver(){
        transform.position = respawn.position;
        playerHealth = maxHealth; 
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
