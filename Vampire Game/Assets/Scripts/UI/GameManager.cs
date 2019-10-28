using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public InventoryUI inventory;
    private void Start() {
        DontDestroyOnLoad(this.gameObject);   
    }

}
