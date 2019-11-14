using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void Start() {
        GameManager.instance.RespawnPoint = gameObject.transform.position;
    }
}
