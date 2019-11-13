using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_Horde_AI : MonoBehaviour
{
    private bool bossFightStarted = false;

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

    //dive attack variables
    private bool isDoingDivingAttack = false;
    [SerializeField] Transform diveLocation;
    [SerializeField] GameObject platform;
    private bool hasDove = false;
    private bool readyToJump = false;
    private bool isFalling = false;
    
    //bite attack cooldown
    [SerializeField] float biteAttackCooldown;

    //debugging stuff
    [SerializeField] bool halfHealthTest = false;


    [SerializeField] Rigidbody2D ratRigidBody;
    [SerializeField] int forceJumpValue;
    [SerializeField] float attackTimeDecreaseInPhase2;


    //level change
    [SerializeField] GameObject[] phaseOneTiles;
    [SerializeField] GameObject[] phaseTwoTiles;
    private GameObject player;
    private Health_Script playerHealthScript;

    //warnings
    [SerializeField] GameObject moveWarning;
    [SerializeField] GameObject spawnWarning;
    [SerializeField] GameObject diveWarning;

    private PolygonCollider2D hitBox;
    [SerializeField] PolygonCollider2D hurtBox;

    [SerializeField] GameObject fallingPipes;

    //animation shit
    private Animator bossAnimation;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GetComponent<Health_Script>();
        ratRigidBody= GetComponent<Rigidbody2D>();
        bossAnimation = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player");
        playerHealthScript = player.GetComponent<Health_Script>();
        moveSelected = -1;
        hitBox = GetComponent<PolygonCollider2D>();
    }

/*
At some point (probably after animations come in for the rat boss) I need to hook up an animator to the boss and make the different placeholder warnings into animations?
or maybe I should keep the warnings in, ill see. Either way I need to implement the bite attack when the player gets/stays too close to the rat for too long.
 */


    // Update is called once per frame
    void Update()
    {
        //debug method to check if the abilities when he is at half hp works correctly
        if(halfHealthTest)
        {
            atHalfHealth();
            halfHealthTest = false;
        }
        if(bossFightStarted)
        {


            if(healthScript.getHealthPercent() <= .5)
            {
                Debug.Log(healthScript.getHealthPercent());
                atHalfHealth();
            }

            time += Time.deltaTime;
            staggerTime += Time.deltaTime;
            if(!belowHalfHealth)
            {
                if(time > attackTimer - 1.5 && moveSelected == -1)
                {
                    moveSelected = Random.Range(1, moves);
                    if(moveSelected == 2)
                    {
                        moveWarning.SetActive(true);
                        spawnWarning.SetActive(false);
                    }
                    else
                    {
                        spawnWarning.SetActive(true);
                        moveWarning.SetActive(false);
                    }
                }
                if (time > attackTimer)
                {
                    moveWarning.SetActive(false);
                    spawnWarning.SetActive(false);
                    
                    time = 0f;
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
                                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3(Random.Range(velMin, velMax), Random.Range(velMin, velMax) / 2, 0f));
                                }
                                else if (transform.position.x >= originalLocation.transform.position.x)
                                {
                                    spawnedRat.GetComponent<Rigidbody2D>().velocity = (new Vector3((Random.Range(-velMin, -velMax)), Random.Range(velMin, velMax) / 2, 0f));
                                }
                            }
                            break;
                        case 2:         //Move attack
                            //Debug.Log("attack2");
                            if (!movingLeft && !movingBack)
                            {
                                if (transform.position.x <= leftLocation.transform.position.x)
                                {
                                    bossAnimation.SetInteger("StateInt", 1);
                                    movingBack = true;
                                }
                                else if (transform.position.x >= originalLocation.transform.position.x)
                                {
                                    bossAnimation.SetInteger("StateInt", 1);
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
                    moveSelected = -1;

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

/*
I need to implement a case in the attack switch so that it can choose to activate its dive attack
*/

            else if (!isDoingDivingAttack)
            {
                //placeholder warning
                if(time > attackTimer/attackTimeDecreaseInPhase2 - 1)
                {
                    moveWarning.SetActive(true);
                }
                if(time > attackTimer/attackTimeDecreaseInPhase2 - .5)
                {
                    bossAnimation.SetInteger("StateInt", 2);
                    ratRigidBody.AddForce(transform.up * forceJumpValue);
                }

                //half health attack timer is shorter
                if (time > attackTimer/attackTimeDecreaseInPhase2)
                {
                    moveWarning.SetActive(false);
                    spawnWarning.SetActive(false);
                    diveWarning.SetActive(false);
                    time = 0f;
                    moveSelected = Random.Range(1, moves);
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
                        case 3:
                            //implement dive attack here
                            isDoingDivingAttack = true;
                            break;
                        default:
                            Debug.Log("Selected move #" + moveSelected);
                            break;
                    }
                    //Every time the boss makes a move it should jump up in the air and move to the opposite side of the arena, unless it is doing the dive attack
                    if (!movingLeft && !movingBack)
                    {
                        if (transform.position.x <= leftLocation.transform.position.x + 1)
                        {
                            movingBack = true;
                        }
                        else if (transform.position.x >= originalLocation.transform.position.x - 1)
                        {
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

            //Do the dive attack logic here.
            //the attack sCaution1hould first make the rat dive under the water.
            //It should then save the current x location of the player and move under neath it.
            //once it reaches there, it should give a warning to let the player know that it is there.
            //then it should jump straight out of the water. After that, it should stay above the water and move to the other side of the arena after a moment of delay.
            else        
            {

                if(!hasDove && !readyToJump && !isFalling)
                {
                    platform.SetActive(false);
                    transform.position = Vector3.MoveTowards
                            (
                                new Vector3(transform.position.x, transform.position.y, 0),
                                new Vector3(transform.position.x, diveLocation.position.y, 0),
                                16 * Time.deltaTime
                            );
                    if(time > attackTimer/(2*attackTimeDecreaseInPhase2))
                    {
                        time = 0f;
                        hasDove = true;
                    }
                }
                else if(hasDove && !readyToJump && !isFalling)
                {
                    bossAnimation.SetInteger("StateInt", 0);
                    transform.position = Vector3.MoveTowards
                    (
                        new Vector3(transform.position.x, transform.position.y, 0),
                        new Vector3(player.transform.position.x, diveLocation.position.y, 0),
                        16 * Time.deltaTime
                    );
                    if(time > attackTimer/(2*attackTimeDecreaseInPhase2) - 1)
                    {
                        diveWarning.SetActive(true);
                    }
                    if(time > attackTimer/(2*attackTimeDecreaseInPhase2))
                    {
                        diveWarning.SetActive(false);
                        time = 0f;
                        readyToJump = true;
                    }
                }
                else if(hasDove && readyToJump && !isFalling)
                {
                    bossAnimation.SetInteger("StateInt", 2);
                    ratRigidBody.AddForce(transform.up * forceJumpValue);
                    if(time > .5f)
                    {
                        time = 0f;
                        isFalling = true;
                    }
                }
                else
                {   
                    platform.SetActive(true);
                    if(time >= 2f)
                    {
                    moveWarning.SetActive(true);
                    }
                    if(time >= 3f)
                    {
                        hasDove = false;
                        readyToJump = false;
                        isFalling = false;
                        isDoingDivingAttack = false;
                    }
                }
            }

            if(!isDoingDivingAttack && movingLeft)
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
                    bossAnimation.SetInteger("StateInt", 0);
                    movingLeft = false;
                    //movingBack = true;
                }
            }
            else if(!isDoingDivingAttack && movingBack)
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
                    bossAnimation.SetInteger("StateInt", 0);
                    movingLeft = false;
                    movingBack = false;
                }
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
        Debug.Log(belowHalfHealth);
        if(!belowHalfHealth)
        {
            stopAI();
            Cutscene();
            for (int i = 0; i < ratz.Count; ++i)
            {
                if (ratz[i] != null)
                {
                    ratz[i].GetComponent<Health_Script>().dealDamage(int.MaxValue);       //gotta change this into something that doesnt cause bleeding
                }
                else
                {
                    ratz.RemoveAt(i);
                }
            }
        } 
    }

    public float getBiteCooldown()
    {
        return biteAttackCooldown;
    }

    public void startFight()
    {
        bossFightStarted = true;
        hitBox.enabled = true;
        hurtBox.enabled = true;
    }

    public void stopAI()
    {
        bossFightStarted = false;
        hitBox.enabled = false;
        hurtBox.enabled = false;
    }

    public bool getBossFightState()
    {
        return bossFightStarted;
    }

    public void Cutscene()
    {
        StartCoroutine(cutsceneDelay());
        
    }

    IEnumerator cutsceneDelay()
    {
        ratRigidBody.AddForce(transform.up * forceJumpValue * 40);
        yield return new WaitForSeconds(.5f);
        bossAnimation.SetInteger("StateInt", 3);
        yield return new WaitForSeconds(.1f);
        fallingPipes.GetComponent<Collider2D>().enabled = false;
        fallingPipes.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(1f);
        for(int i = 0; i < walls.Length; ++i)
        {
            walls[i].moveRatWall2();
        }
        yield return new WaitForSeconds(7f);
        Destroy(fallingPipes);
        startFight();
        for(int i = 0; i < spikes.Length; ++i)
        {
            spikes[i].GetComponent<Move_Whole_Spike>().moveUp(spikeLocationAtHalfHealth);
        }
        for(int i = 0; i < phaseOneTiles.Length; ++i)
        {
            phaseOneTiles[i].SetActive(false);
        }
        for(int i = 0; i < phaseTwoTiles.Length; ++i)
        {
            phaseTwoTiles[i].SetActive(true);
        }
        
        belowHalfHealth = true;
    }



}
