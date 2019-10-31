using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;
    public List<Item> items = new List<Item>();

    private void Awake() {
        instance = this;    
    }
    public void Add(Item item){
        items.Add(item);
    }

    public void Remove(Item item){
        items.Remove(item);
    }
}
