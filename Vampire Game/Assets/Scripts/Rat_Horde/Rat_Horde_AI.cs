﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Horde_AI : MonoBehaviour
{
    [SerializeField] int moves;
    private int moveSelected;
    [SerializeField] Rat_Wall[] walls;
    private float time = 0f;
    private float staggerTime = 0f;
    private bool belowHalfHealth = false;
    [SerializeField] float attackTimer;
    private Health_Script healthScript;
    
    //spike attack variables
    [SerializeField] GameObject[] spikes;
    [SerializeField] List<int> spikeNumberList;
    [SerializeField] Transform spikeLocationAtHalfHealth;

    //move attack variables
    [SerializeField] Transform leftLocation;
    [SerializeField] Transform originalLocation;
    private bool movingLeft = false;
    private bool movingBack = false;

    //rat spawning attack
    [SerializeField] int numRatz;
    [SerializeField] List<GameObject> ratz;
    [SerializeField] GameObject rat;
    [SerializeField] float velMax;
    [SerializeField] float velMin;
    private Vector3 spawnOffset;
    [SerializeField] int damagePerTickToSpawnedRats;
    private bool spawning = false;
    private int randStaggeredRatz;

    //debugging stuff
    [SerializeField] bool halfHealthTest = false;


    [SerializeField] Rigidbody2D ratRigidBody;
    [SerializeField] int forceJumpValue;
    [SerializeField] float attackTimeDecreaseInPhase2;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GetComponent<Health_Script>();
        ratRigidBody= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //debug method to check if the abilities when he is at half hp works correctly
        if(halfHealthTest)
        {
            atHalfHealth();
            halfHealthTest = false;
        }

        if(healthScript.getHealthPercent() <= .5)
        {
            Debug.Log(healthScript.getHealthPercent());
            atHalfHealth();
        }

        time += Time.deltaTime;
        staggerTime += Time.deltaTime;
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
                        //spawnOffset = new Vector3(0, 0, 0);
                        int randRats = Random.Range(1, numRatz + 1);
                        for (int i = 0; i < randRats; ++i)
                        {
                            //spawnOffset = new Vector3(0, 2*i, 0);
                            GameObject spawnedRat = Instantiate(rat, transform.position, transform.rotation);
                            ratz.Add(spawnedRat);
                            if (transform.position.x <= leftLocation.transform.position.x)
                            {
                                spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3(Random.Range(velMin, velMax), Random.Range(velMin, velMax)/2, 0f));
                            }
                            else if (transform.position.x >= originalLocation.transform.position.x)
                            {
                                spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3((Random.Range(-velMin, -velMax)), Random.Range(velMin, velMax)/2, 0f));
                            }
                        }
                        break;
                    case 2:         //Move attack
                        //Debug.Log("attack2");
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
                    case 3:         //Staggered Spawn Attack
                        spawnOffset = new Vector3(0, 0, 0);
                        randStaggeredRatz = Random.Range(1, numRatz + 1);
                        spawning = true;
                        break;
                    default:
                        Debug.Log("Selected move #" + moveSelected);
                        break;
                }

                for (int i = 0; i < ratz.Count; ++i)
                {
                    if (ratz[i] != null)
                    {
                        ratz[i].GetComponent<Health_Script>().dealDamage(damagePerTickToSpawnedRats);
                    }
                    else
                    {
                        ratz.RemoveAt(i);
                    }
                }
                
            }

            if (spawning && staggerTime > attackTimer/2 && randStaggeredRatz != 0)
            {
                staggerTime = 0f;
                GameObject spawnedRat = Instantiate(rat, transform.position, transform.rotation);
                ratz.Add(spawnedRat);
                if (transform.position.x <= leftLocation.transform.position.x)
                {
                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3(Random.Range(velMin, velMax), Random.Range(velMin, velMax), 0f));
                }
                else if (transform.position.x >= originalLocation.transform.position.x)
                {
                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3((Random.Range(-velMin, -velMax)), Random.Range(velMin, velMax), 0f));
                }
                randStaggeredRatz -= 1;
            }
            else if(randStaggeredRatz == 0)
            {
                spawning = false;
            }

        }

        else
        {
            //half health attack timer is shorter
            if (time > attackTimer/attackTimeDecreaseInPhase2)
            {
                time = 0f;
                moveSelected = Random.Range(1, moves - 1);
                ResetSpikes();
                int numSpikes = Random.Range(1, spikes.Length/2);
                //spike attack
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
                        //spawnOffset = new Vector3(0, 0, 0);
                        int randRats = Random.Range(1, numRatz + 1);
                        for (int i = 0; i < randRats; ++i)
                        {
                            //spawnOffset = new Vector3(0, 2*i, 0);
                            GameObject spawnedRat = Instantiate(rat, transform.position, transform.rotation);
                            ratz.Add(spawnedRat);
                            if (transform.position.x <= leftLocation.transform.position.x)
                            {
                                spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3(Random.Range(velMin, velMax), Random.Range(velMin, velMax), 0f));
                            }
                            else if (transform.position.x >= originalLocation.transform.position.x)
                            {
                                spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3((Random.Range(-velMin, -velMax)), Random.Range(velMin, velMax), 0f));
                            }
                        }
                        break;
                    case 2:         //Staggered Spawn Attack
                        spawnOffset = new Vector3(0, 0, 0);
                        randStaggeredRatz = Random.Range(1, numRatz + 1);
                        spawning = true;
                        break;
                    default:
                        Debug.Log("Selected move #" + moveSelected);
                        break;
                }

                if (!movingLeft && !movingBack)
                {
                    if (transform.position.x <= leftLocation.transform.position.x + 1)
                    {
                        ratRigidBody.AddForce(transform.up * forceJumpValue);
                        movingBack = true;
                    }
                    else if (transform.position.x >= originalLocation.transform.position.x - 1)
                    {
                        ratRigidBody.AddForce(transform.up * forceJumpValue);
                        movingLeft = true;
                    }
                }

                for (int i = 0; i < ratz.Count; ++i)
                {
                    if (ratz[i] != null)
                    {
                        ratz[i].GetComponent<Health_Script>().dealDamage(damagePerTickToSpawnedRats);       //gotta change this into something that doesnt cause bleeding
                    }
                    else
                    {
                        ratz.RemoveAt(i);
                    }
                }

            }

            if (spawning && staggerTime > attackTimer / 3 && randStaggeredRatz != 0)
            {
                staggerTime = 0f;
                GameObject spawnedRat = Instantiate(rat, transform.position, transform.rotation);
                ratz.Add(spawnedRat);
                if (transform.position.x <= leftLocation.transform.position.x)
                {
                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3(Random.Range(velMin, velMax), Random.Range(velMin, velMax), 0f));
                }
                else if (transform.position.x >= originalLocation.transform.position.x)
                {
                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3((Random.Range(-velMin, -velMax)), Random.Range(velMin, velMax), 0f));
                }
                randStaggeredRatz -= 1;
            }
            else if (randStaggeredRatz == 0)
            {
                spawning = false;
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
        for(int i = 0; i < spikes.Length; ++i)
        {
            spikes[i].GetComponent<Move_Whole_Spike>().moveUp(spikeLocationAtHalfHealth);
        }
    }
}
