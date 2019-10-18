using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look_Script : MonoBehaviour
{
    public float LookDistance = 3;
    private float LookDirection;
    private float lastLookV;
    private float lastLookH;



    // Update is called once per frame
    void Update()
    {
        LookDirection = Input.GetAxisRaw("Vertical");
        if(LookDirection == lastLookV - 1)
        {
            transform.position = transform.position + new Vector3(0, -LookDistance, 0);
        }else if (LookDirection == lastLookV + 1)
        {
            transform.position = transform.position + new Vector3(0, LookDistance, 0);
        }
        lastLookV = LookDirection;

        
    }
}
