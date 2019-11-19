﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public Item item;
    void Pickup()
    {
        InventoryUI.instance.Add(item);
        Debug.Log(item.name + " was pciked up.");
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Pickup();
        }
    }
}
