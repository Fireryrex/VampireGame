
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    

    public Vector3 offset;
    public float cameraMoveSpeed = 1f;


    private void FixedUpdate()
    {
        Vector3 cameraDir = ((target.position + offset) - transform.position).normalized;
        float distance = Vector3.Distance((target.position + offset), transform.position);


        transform.position = (transform.position + cameraDir * distance * cameraMoveSpeed * Time.deltaTime);

        
    }
}
