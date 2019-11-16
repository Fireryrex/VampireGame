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

    private void Awake() {
        instance = this;    
    }
    private void Start() {
        
        Debug.Log(inventory.Length);
    }
    public void Add(Item item){
        items.Add(item);
        inventoryChanged = true;
        currentItem += 1;
    }
    public void updateInventory(){
        if (inventoryChanged == true){
            inventoryChanged = false;
            inventory = GameObject.FindGameObjectsWithTag("inventoryItem");
            for (int i = 0; i < currentItem; i++){
                inventory[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = items[i].icon;
            }
        }
    }
    public void Remove(Item item){
        items.Remove(item);
    }
}
