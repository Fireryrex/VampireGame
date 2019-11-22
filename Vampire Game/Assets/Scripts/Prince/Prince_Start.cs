using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince_Start : MonoBehaviour
{
    //Gate stuff
    [SerializeField] Transform gateActive;
    [SerializeField] Transform gateInactive;
    [SerializeField] GameObject gate;
    private bool gateIsActive = false;

    //Prince ai stuff
    [SerializeField] Prince_AI princeAI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gateIsActive)
        {
            gate.transform.position = Vector3.MoveTowards
                    (
                        new Vector3(gate.transform.position.x, gate.transform.position.y, 0),
                        new Vector3(gate.transform.position.x, gateActive.transform.position.y, 0),
                        16 * Time.deltaTime
                    );
        }
        else
        {
            gate.transform.position = Vector3.MoveTowards
                 (
                        new Vector3(gate.transform.position.x, gate.transform.position.y, 0),
                        new Vector3(gate.transform.position.x, gateInactive.transform.position.y, 0),
                        Time.deltaTime
                    );
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            gateIsActive = true;
            princeAI.startFight(3);
        }
    }
}
