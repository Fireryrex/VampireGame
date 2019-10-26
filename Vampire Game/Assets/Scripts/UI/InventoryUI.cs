using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    private void Awake() {
        instance = this;    
    }
    public List<Item> items = new List<Item>();

    public void Add(Item item){
        items.Add(item);
    }

    public void Remove(Item item){
        items.Remove(item);
    }
}
