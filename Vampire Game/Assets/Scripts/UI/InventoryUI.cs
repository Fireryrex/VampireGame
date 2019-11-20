using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public List<Item> items = new List<Item>(); //The Scriptable items that have been picked up
    public GameObject[] inventory = new GameObject[6]; // The list of inventory slots that are in the UI.
    private int currentItem = 0;
    public static bool inventoryChanged = false;
    GameObject player;

    private void Awake() {
        instance = this;    
    }
    private void Start() {
        
        Debug.Log(inventory.Length);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Add(Item item){
        items.Add(item);
        inventoryChanged = true;
        currentItem += 1;
        unlock(item.ability);
    }
    public void updateInventory(){
        if (inventoryChanged == true){
            inventoryChanged = false;
            inventory = GameObject.FindGameObjectsWithTag("inventoryItem");
            for (int i = 0; i < currentItem; i++){
                inventory[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = items[i].icon;
                inventory[i].GetComponent<InventorySlot>().AddItem(items[i]);
                
            }
        }
    }
    public void Remove(Item item){
        items.Remove(item);
    }

    private void unlock(string ability){
        if (ability == "attack"){
            player.GetComponent<VampireController2>().unlockAttack();
        }
        else if(ability == "attack2"){
            player.GetComponent<VampireController2>().unlockAttack2();
        }
        else if (ability == "attack3"){
            player.GetComponent<VampireController2>().unlockAttack3();
        }
        else if (ability == "dash"){
            player.GetComponent<VampireController2>().unlockDash();
        }
        else if (ability == "double jump"){
            player.GetComponent<VampireController2>().unlockDoubleJump();
        }
    }
}
