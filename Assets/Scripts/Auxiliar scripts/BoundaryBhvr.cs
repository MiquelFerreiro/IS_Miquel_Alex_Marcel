using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryBhvr : MonoBehaviour
{
    public Vector3 push_direction = Vector3.zero;
    public static float force_factor = 10f;

    private void Start()
    {
        push_direction = push_direction.normalized; 
    }


    private void OnTriggerStay(Collider other)
    {
        
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Rigidbody>().AddForce(push_direction * force_factor); 

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            float signx = Mathf.Sign(rb.velocity.x);
            float signz = Mathf.Sign(rb.velocity.z);
            rb.velocity = new Vector3(signx * Mathf.Sqrt(signx * rb.velocity.x), 0f, signz * Mathf.Sqrt(signz * rb.velocity.z)); 
        }
    }


}
