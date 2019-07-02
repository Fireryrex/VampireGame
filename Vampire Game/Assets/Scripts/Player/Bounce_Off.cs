using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce_Off : MonoBehaviour
{

    public Rigidbody2D rb;
    private float jumpSpeed;
    public Vampire_Controller vp;

    // Start is called before the first frame update
    void Start()
    {
        jumpSpeed = vp.getJumpSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(rb.velocity.x, jumpSpeed));
        }
    }
}
