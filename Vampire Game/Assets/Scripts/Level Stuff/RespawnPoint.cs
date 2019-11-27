using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPoint : MonoBehaviour
{
    private Animator checkpointAnimator;
    private Vector2 respawnPosition;


    private void Start()
    {
        checkpointAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (checkpointAnimator.GetBool("isActive") == false)
        {
            respawnPosition = gameObject.transform.position;
            if (GameManager.instance.RespawnPoint == respawnPosition)
            {
                checkpointAnimator.SetBool("isActive", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            checkpointAnimator.SetBool("isActive", true);
            GameManager.instance.RespawnPoint = gameObject.transform.position;
            GameManager.instance.respawnLevel = SceneManager.GetActiveScene().name;
            other.GetComponent<Health_Script>().respawnPoint = SceneManager.GetActiveScene().name;
            other.GetComponent<Health_Script>().respawnPosition = gameObject.transform.position;
        }
    }
}
