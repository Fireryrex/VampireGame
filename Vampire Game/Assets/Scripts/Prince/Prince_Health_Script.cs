using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prince_Health_Script : Health_Script
{
    [SerializeField] ParticleSystem bloodSpray;

    // Start is called before the first frame update
    void Start()
    {

    }

// Update is called once per frame
void Update()
    {
        
    }

    public override void dealDamage(int damage)
    {
        if (coolDown == 0)
        {
            coolDown = coolDownTime;
            particleDamageTrigger();
            health -= damage;
            if (health <= 0)
            {
                //Instantiate(bloodSpray, transform.position, Quaternion.identity, gameObject.transform);
                //play the death cinematic        
            }

        }
    }
}
