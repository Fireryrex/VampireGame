using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : MonoBehaviour
{
    private bool movingRight;
    public Transform groundDetection;
    public Transform wallDetection;

    private Rigidbody2D rb;
    public float forceMultiplyer = 100f;
    public float distance = 2f; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, distance);


        if (groundInfo.collider != null || groundInfo.collider == true)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }

        rb.velocity = rb.velocity * new Vector3(0, 1, 0);
        rb.AddForce(transform.right * Time.deltaTime * forceMultiplyer);
    }
}
