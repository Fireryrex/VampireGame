using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Horde_AI : MonoBehaviour
{
    public int moves;
    private int moveSelected;
    public Rat_Wall[] walls;
    private float time = 0f;
    private bool belowHalfHealth = false;
    public float attackTimer;
    
    //spike attack variables
    public GameObject[] spikes;
    public List<int> spikeNumberList;

    //move attack variables
    public Transform leftLocation;
    public Transform originalLocation;
    private bool movingLeft = false;
    private bool movingBack = false;

    //rat spawning attack
    public GameObject[] ratz;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(!belowHalfHealth)
        {
            if (time > attackTimer)
            {
                time = 0f;
                moveSelected = Random.Range(1, moves);
                ResetSpikes();
                int numSpikes = Random.Range(1, spikes.Length);
                for (int i = 0; i < numSpikes; ++i)
                {
                    int chosenSpike = spikeNumberList[Random.Range(0, spikeNumberList.Count)];
                    spikeNumberList.Remove(chosenSpike);
                    spikes[chosenSpike].SetActive(true);
                    spikes[chosenSpike].GetComponentInChildren<Spike_Rat>().Appear();
                }
                switch (moveSelected)
                {
                    case 1:         //Spawn attack

                        break;
                    case 2:         //Move attack
                        Debug.Log("attack2");
                        if(!movingLeft && !movingBack)
                        {
                            if (transform.position.x <= leftLocation.transform.position.x)
                            {
                                movingBack = true;
                            }
                            else if (transform.position.x >= originalLocation.transform.position.x)
                            {
                                movingLeft = true;
                            }
                        }
                        break;
                    case 3:
                        break;
                    default:
                        Debug.Log("Selected move #" + moveSelected);
                        break;
                }

            }
        }

        if(movingLeft)
        {
            transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(leftLocation.transform.position.x, transform.position.y, 0),
                        16 * Time.deltaTime
                    );
            if(transform.position.x <= leftLocation.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
                movingLeft = false;
                //movingBack = true;
            }
        }
        else if(movingBack)
        {
            transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(originalLocation.transform.position.x, transform.position.y, 0),
                        16 * Time.deltaTime
                    );
            if (transform.position.x >= originalLocation.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                movingLeft = false;
                movingBack = false;
            }
        }
    }

    private void ResetSpikes()
    {
        spikeNumberList.Clear();
        spikeNumberList.Add(0);
        spikeNumberList.Add(1);
        spikeNumberList.Add(2);
        spikeNumberList.Add(3);
    }

    public void atHalfHealth()
    {
        belowHalfHealth = true;
        for(int i = 0; i < walls.Length; ++i)
        {
            walls[i].moveRatWall2();
        }
    }
}
