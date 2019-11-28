using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chakram_Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] Vector2 initialVel;
    [SerializeField] Vector2 forceAdded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(forceAdded);
        rb.AddTorque(10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Destroy(gameObject);
        }
    }
}
