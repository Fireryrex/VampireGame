using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look_Script : MonoBehaviour
{
    public float LookDirection;
    public float lastLook;

    
    // Update is called once per frame
    void Update()
    {
        LookDirection = Input.GetAxisRaw("Vertical");
        if(LookDirection == lastLook - 1)
        {
            transform.position = transform.position + new Vector3(0, -3, 0);
        }else if (LookDirection == lastLook + 1)
        {
            transform.position = transform.position + new Vector3(0, 3, 0);
        }
        lastLook = LookDirection;
    }
}
