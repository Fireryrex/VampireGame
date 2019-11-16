using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject frame;
    private Item item;
    public void AddItem(Item newItem){
        item = newItem;
    }
}
