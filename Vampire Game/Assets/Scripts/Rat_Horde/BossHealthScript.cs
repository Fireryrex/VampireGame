﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthScript : Health_Script
{
    private Rat_Horde_AI bossAI;
    [SerializeField] ParticleSystem bloodSpray;
    

    // Start is called before the first frame update
    void Start()
    {
        bossAI = GetComponent<Rat_Horde_AI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void dealDamage(int damage)
    {
        if(coolDown == 0)
        {
            Instantiate(bloodParticleSystem, gameObject.transform.position, Quaternion.identity);
            coolDown = coolDownTime;
            health -= damage;
            if (health <= 0)
            {
                bossAI.stopAI();
                Instantiate(bloodSpray, transform.position + new Vector3(0, 2, 0), Quaternion.identity, gameObject.transform);
                bossAI.lowerWalls();
                bossAI.resetCamera();
                //play the death cinematic        
            }

        }
    }
}
