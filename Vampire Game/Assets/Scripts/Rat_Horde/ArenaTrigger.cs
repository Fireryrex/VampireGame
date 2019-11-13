using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaTrigger : MonoBehaviour
{
    [SerializeField] Rat_Horde_AI bossAI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log( collision.tag);
        if(collision.CompareTag("Falling Object"))
        {
            bossAI.changeArena();
        }
    }
}
