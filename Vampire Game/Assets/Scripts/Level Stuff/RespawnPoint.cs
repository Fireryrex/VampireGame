using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        GameManager.instance.RespawnPoint = gameObject.transform.position;
        GameManager.instance.respawnLevel = SceneManager.GetActiveScene().name;
    }
}
