using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    public DistanceJoint2D ropeJoint;
    public Rigidbody2D RigidBody2D;
    public int projectileSpeed;
 //   private float movementSmoothing = .05f;
    public Rigidbody2D PCRigidBody2D;
    public Vector2 playerPosition;
    private bool isShooting = false;
    public Character_Controller controller;

    /*
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Character_Controller playerMovement;
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;
    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePositions = new List<Vector2>();
    */

    private Vector3 m_Velocity = Vector3.zero;

    private void Awake()
    {
        ropeJoint.enabled = false;
        RigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = PCRigidBody2D.transform.position;
        if (!isShooting)
        {
            ropeJoint.enabled = false;
            RigidBody2D.bodyType = RigidbodyType2D.Kinematic;
            transform.position = playerPosition;
        }
    }

    public void Shoot(float keyXInput, float keyYInput)
    {
        ropeJoint.enabled = false;
        if(isShooting)
        {
            RigidBody2D.bodyType = RigidbodyType2D.Kinematic;
            transform.position = playerPosition;
        }
        RigidBody2D.velocity = new Vector2(projectileSpeed * 10f * keyXInput, projectileSpeed * 10f * keyYInput);
        isShooting = true;
    }

    public void Detatch()
    {
        if (ropeJoint.enabled)
        {
            controller.resetAirMovements();
        }
        ropeJoint.enabled = false;
        RigidBody2D.bodyType = RigidbodyType2D.Kinematic;
        isShooting = false;
        RigidBody2D.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RigidBody2D.bodyType = RigidbodyType2D.Static;
        ropeJoint.enabled = true;
    }
}
