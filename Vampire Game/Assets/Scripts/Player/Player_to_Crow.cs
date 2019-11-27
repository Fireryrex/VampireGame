using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_to_Crow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject crow;
    [SerializeField] Rigidbody2D crowBody;
    [SerializeField] CrowFlyScript crowScript;
    [SerializeField] Transform crowParent;

    private Pathfinding.AIDestinationSetter targetingScript;
    [SerializeField] Transform crowTarget1;
    [SerializeField] Transform crowTarget2;

    private void Start()
    {
        findCrow();
    }

    public void findCrow()
    {
        crow = GameObject.FindWithTag("Crow");
        crowScript = crow.GetComponentInChildren<CrowFlyScript>();
        targetingScript = crow.GetComponentInParent<Pathfinding.AIDestinationSetter>();
        crowBody = crow.GetComponentInParent<Rigidbody2D>();
        crowParent = crow.transform.parent.transform;
    }

    public void Update()
    {

    }

    public void crowFlap()
    {
        //Debug.Log("lkajsdfkjasdhfaksjdhflkasjdhflkasdjjfhlksdjjfhksdjfhaslkjfjhaslkdjfhasdlkjfh");
        targetingScript.target = crowTarget2;
        //Debug.Log("crowScript.isTouchingPlayer() =" + crowScript.isTouchingPlayer());
        if (crowScript.isTouchingPlayer())
        {
            crowBody.AddForce(Vector2.up * 2000);
            crowScript.setTouchingPlayer(false);
        }
    }

    public void crowStand()
    {
        targetingScript.target = crowTarget1;
    }

    public Transform returnCrowTarget1()
    {
        return crowTarget1;
    }

    public void crowUpdatePosition()
    {
        crowParent.position = crowTarget1.position;
    }
}
