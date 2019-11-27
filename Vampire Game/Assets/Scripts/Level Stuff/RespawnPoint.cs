using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.RespawnPoint = gameObject.transform.position;
            GameManager.instance.respawnLevel = SceneManager.GetActiveScene().name;
            other.GetComponent<Health_Script>().respawnPoint = SceneManager.GetActiveScene().name;
            other.GetComponent<Health_Script>().respawnPosition = gameObject.transform.position;
        }
    }
}
