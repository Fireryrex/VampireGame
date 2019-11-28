using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear_Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float velAdded;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (velAdded > 100)
        {
            velAdded = 100;
        }
        rb.velocity = new Vector2(velAdded, 0);
        velAdded *= 1.1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            Destroy(gameObject);
        }
    }
}
