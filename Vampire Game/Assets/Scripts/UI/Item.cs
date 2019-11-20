using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
//Item object that can be added to GameObject.
{
    public string itemName = "New Item";
    public Sprite icon = null; //This icon will be displayed both in game and inventory
    public string itemDescription = "New description.";
    public string ability = "define Ability";
}
