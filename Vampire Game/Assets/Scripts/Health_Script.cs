using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Script : MonoBehaviour
{
    public int health;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Decreases the characters health by damage
    public void dealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Am Dead");
            //call some death function
        }
    }

    public void heal(int healAmt)
    {
        health += healAmt;
    }
}