using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Item item;
    void Pickup(string ability)
    {
        if(ability == "increaseMaxHealth"){
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health_Script>().maxHealth += 1;
            GameObject.Find("Heart Holder").GetComponent<heartHolder>().createHeart();
            GameManager.instance.FIllHeart();
            Destroy(gameObject);
            

        }
        else{
        InventoryUI.instance.Add(item);
        Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Pickup(gameObject.GetComponent<ItemHandler>().item.ability);
        }
    }
}
