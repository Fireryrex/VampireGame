using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    private Item item;
    public void AddItem(Item newItem){
        item = newItem;

    }
    public void displayText(){
        GameObject.Find("ItemDescriptions").GetComponent<TextMeshProUGUI>().text = item.itemDescription;
    }
}
