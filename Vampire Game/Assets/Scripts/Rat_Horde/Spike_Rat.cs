using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Rat : MonoBehaviour
{
    private bool isAppearing = false;
    private float time = 0f;
    public float warningTime;
    public float timeOut;
    public Transform moveLocation;
    public Transform returnTo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAppearing)
        {
            time += Time.deltaTime;
            if(time >= warningTime)
            {
                this.gameObject.transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(moveLocation.transform.position.x, moveLocation.transform.position.y, 0),
                        20 * Time.deltaTime
                    );
            }
            if(time >= warningTime + timeOut)
            {
                isAppearing = false;
            }
        }
        else
        {
            this.gameObject.transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(returnTo.transform.position.x, returnTo.transform.position.y, 0),
                        20 * Time.deltaTime
                    );
        }
    }

    public void Appear()
    {
        time = 0f;
        isAppearing = true;
    }
}
